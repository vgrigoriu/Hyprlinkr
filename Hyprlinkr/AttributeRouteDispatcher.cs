namespace Ploeh.Hyprlinkr
{
    public class AttributeRouteDispatcher: CompositeRouteDispatcher
    {
        public AttributeRouteDispatcher() : base(new []
        {
            new DefaultRouteDispatcher(),
            new DefaultRouteDispatcher("MS_attributerouteWebApi"), 
        })
        {
        }
    }
}
