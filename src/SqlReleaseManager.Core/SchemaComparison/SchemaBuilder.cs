using Microsoft.SqlServer.Dac.Model;

namespace SqlReleaseManager.Core.SchemaComparison;

public class SchemaBuilder
{
    public SchemaBuilder()
    {
    }

    public List<ProcedureObject> Procedures { get; set; } = new List<ProcedureObject>();
    public List<TableObject> Tables { get; set; } = new List<TableObject>();
    public List<ViewObject> Views { get; set; } = new List<ViewObject>();

    public Schema Build()
    {
        return new Schema()
        {
            DatabaseName = DatabaseName,
            Tables = Tables,
            Views = Views,
            Procedures = Procedures,
        };
    }

    public SchemaBuilder WithDatabase(string databaseName)
    {
        DatabaseName = databaseName;

        return this;
    }

    public string DatabaseName { get; set; }

    public SchemaBuilder ForTSqlModel(TSqlModel model)
    {
        foreach (var table in model.GetObjects(DacQueryScopes.UserDefined, Table.TypeClass))
        {
            var tableObject = SchemaObject.FromTSqlObject<TableObject>(table);
            var children = table.GetChildren(DacQueryScopes.UserDefined);
            tableObject.Columns = GetColumns(table);
            tableObject.Constraints = GetConstraints(table);

            Tables.Add(tableObject);
        }


        foreach (var view in model.GetObjects(DacQueryScopes.UserDefined, View.TypeClass))
        {
            var viewObject = SchemaObject.FromTSqlObject<ViewObject>(view);
            viewObject.Columns = GetColumns(view);
            Views.Add(viewObject);
        }


        foreach (var procedure in model.GetObjects(DacQueryScopes.UserDefined, Procedure.TypeClass))
        {
            var procedureObject = SchemaObject.FromTSqlObject<ProcedureObject>(procedure);
            var children = procedure.GetChildren(DacQueryScopes.UserDefined);
            procedureObject.Parameters = children.Where(o => o.ObjectType == Parameter.TypeClass)
                .Select(SchemaObject.FromTSqlObject<ParameterObject>).ToList();
            Procedures.Add(procedureObject);
        }


        return this;
    }

    private IEnumerable<ColumnObject> GetColumns(TSqlObject table)
    {
        return table.GetChildren(DacQueryScopes.UserDefined).Where(o => o.ObjectType == Column.TypeClass)
            .Select(ColumnObject.FromTSqlObject);
    }

    private IEnumerable<ConstraintObject> GetConstraints(TSqlObject obj)
    {
        return obj.GetChildren(DacQueryScopes.UserDefined).Where(o =>
            o.ObjectType == DefaultConstraint.TypeClass ||
            o.ObjectType == PrimaryKeyConstraint.TypeClass ||
            o.ObjectType == ForeignKeyConstraint.TypeClass ||
            o.ObjectType == CheckConstraint.TypeClass ||
            o.ObjectType == UniqueConstraint.TypeClass
        ).Select(o => SchemaObject.FromTSqlObject<ConstraintObject>(o) with
        {
            Type = GetConstraintType(o)
        });
    }

    private SchemaBuilder WithTable(TableObject tableObject)
    {
        Tables.Add(tableObject);

        return this;
    }

    private static ConstraintType GetConstraintType(TSqlObject o)
    {
        if (o.ObjectType == DefaultConstraint.TypeClass)
        {
            return ConstraintType.Default;
        }

        if (o.ObjectType == PrimaryKeyConstraint.TypeClass)
        {
            return ConstraintType.PrimaryKey;
        }

        if (o.ObjectType == ForeignKeyConstraint.TypeClass)
        {
            return ConstraintType.ForeignKey;
        }

        if (o.ObjectType == CheckConstraint.TypeClass)
        {
            return ConstraintType.Check;
        }

        if (o.ObjectType == UniqueConstraint.TypeClass)
        {
            return ConstraintType.Unique;
        }

        throw new Exception("Unknown constraint type");
    }
}