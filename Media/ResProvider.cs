


using Media.Resources;

namespace Media
{
    public class ResProvider 
    {
        static ProjectResources resources = new ProjectResources();

        public ProjectResources ProjectResources
        {
            get
            {
                return resources;
            }
        }
      
    }
}
