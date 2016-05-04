using System.Collections;
using System.Collections.Generic;
public interface IBuildable<P>
{
    void Create(Prop prop, P param);
}
public class BuildData
{
    public uint ID { get; private set; }
    public Prop Prop { get; private set; }

    private BuildData(uint id, string[] properties)
    {
        ID = id;
        Prop = new Prop(properties);
    }

    private BuildData(uint id, Prop prop)
    {
        ID = id;
        Prop = prop;
    }

    public static List<BuildData> NonParamDatas(params uint[] datas)
    {
        List<BuildData> list = new List<BuildData>();
        foreach (var id in datas)
        {
            list.Add(new BuildData(id, new string[0]));
        }
        return list;
    }

    public static List<BuildData> Datas(params BuildData[] datas)
    {
        List<BuildData> list = new List<BuildData>();
        foreach (var data in datas)
        {
            list.Add(data);
        }
        return list;
    }

    public static BuildData Trigger(uint id)
    {
        return new BuildData(id, new string[0]);
    }
}
public class TBuilder<I, P>
{
    Dictionary<uint, Builder> mBuilders = new Dictionary<uint, Builder>();

    public I Build(BuildData data, P parameter)
    {
        return mBuilders[data.ID].Build(data.Prop, parameter);
    }

    protected void Add<T>(uint id) where T : I, IBuildable<P>, new()
    {
        mBuilders.Add(id, new Builder<T>());
    }

    public int GetBuildersCountTBuilder()
    {
        return mBuilders.Count;
    }

    abstract class Builder
    {
        public abstract I Build(Prop Prop, P parameter);
    }

    class Builder<T> : Builder where T : I, IBuildable<P>, new()
    {
        public override I Build(Prop Prop, P parameter)
        {
            T buildable = new T();
            buildable.Create(Prop, parameter);
            return buildable;
        }
    }
}
