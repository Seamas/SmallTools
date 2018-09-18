using System;
using System.Threading.Tasks;

namespace BingWallpaper
{
    public interface IWallpaper
    {
        string Host { get; }

        void Save();

        Task SaveAsync();
    }
}