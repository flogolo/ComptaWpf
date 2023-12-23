using CommonLibrary.Tools;

namespace MaCompta.ViewModels
{
    public class DetailStatMulti
    {
        public DetailStatMulti()
        {
            Stats = new SortableObservableCollection<DetailStatSerie>();
        }

        public string Libelle { get; set; }
        public long Id { get; set; }

        //        public Brush FavoriteColor { get; set; }
        public SortableObservableCollection<DetailStatSerie> Stats { get; set; }
    }

}
