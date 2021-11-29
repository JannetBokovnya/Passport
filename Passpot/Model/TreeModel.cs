using System.Collections.ObjectModel;
using Media.Interfaces;
using Passpot.Business;
using Services.ServiceReference;

namespace Passpot.Model
{
    public class TreeModel: ModelBase
    {

        public MainModel MainModel { get; private set; }

        public TreeOpenParam TreeOpenParam { get; private set; }

        public TreeModel(MainModel mainModel)
        {
            MainModel = mainModel;
        }

        public ObservableCollection<TreeFullHierarchyPathToObjectItems> SelectionPath { get; private set; }

        public void InitModel(TreeOpenParam treeOpenParam)
        {

            TreeOpenParam = treeOpenParam;

            SelectionPath = new ObservableCollection<TreeFullHierarchyPathToObjectItems>();

            if (TreeOpenParam.TypeTree == "2")
            {
                ServiceDataClient.GetTreeFullHierarchyPathToObjectCompleted += GetTreeFullHierarchyPathToObjectCompleted;
                ServiceDataClient.GetTreeFullHierarchyPathToObjectAsync(TreeOpenParam.RootKey, GlobalContext.Context);
            }
            else
            {

               // MainModel.NeedOpenTree; 
                MainModel.FirePropertyChanged("TreeModelInited");
            }
        }

        void GetTreeFullHierarchyPathToObjectCompleted(object sender, GetTreeFullHierarchyPathToObjectCompletedEventArgs e)
        {
            ServiceDataClient.GetTreeFullHierarchyPathToObjectCompleted -= GetTreeFullHierarchyPathToObjectCompleted;
            if (e.Result.IsValid)
            {
                if (e.Result.TreeFullHierarchyPathToObjectList.Count > 0)
                {
                    SelectionPath = e.Result.TreeFullHierarchyPathToObjectList;
                    MainModel.FirePropertyChanged("TreeModelInited");
                }
                else
                {
                    MainModel.Report("GetTreeFullHierarchyPathToObject вернул количество записей = " + (e.Result.TreeFullHierarchyPathToObjectList.Count));
                }
            }
            else
            {
                MainModel.Report("GetTreeFullHierarchyPathToObject ошибка = " + e.Error.Message);
            }

        }
 
    }

    
}
