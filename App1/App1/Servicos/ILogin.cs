using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1.Servicos
{
    public interface ILogin
    {
        void SigninButton_Click();
    }
    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
