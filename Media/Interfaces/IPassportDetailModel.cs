
namespace Media.Interfaces
{
	public interface IPassportDetailModel
	{
		IMainModel MainModel { get; }

        //void DeleteMedia(string currentPassportKey, string keyMedia, string nameMedia);
        void Variablesmedia(string currentPassportKey, string keyMedia, string nameMedia);
	}
}
