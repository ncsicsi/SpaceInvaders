using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpaceInvaders.ViewModel
{
    // Nezetmodel ososztaly tipusa
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //peldanyosítas
        protected ViewModelBase() { }

        //tulajdonsag valtozasanak esemenye
        public event PropertyChangedEventHandler PropertyChanged;

        //tulajdonsag valtozsanak ellenorzese
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}