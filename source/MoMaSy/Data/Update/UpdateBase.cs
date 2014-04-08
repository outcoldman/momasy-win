
namespace outcold.MoMaSy.Data.Update
{
    abstract class UpdateBase
    {
        public abstract int GetUpdateVersion();

        public abstract bool Update(Storage storage);
    }
}
