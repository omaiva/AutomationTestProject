using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationTestProject.Application.Interfaces
{
    public interface IImageStorageService
    {
        Task SaveProfileImage(string imgUrl);
    }
}
