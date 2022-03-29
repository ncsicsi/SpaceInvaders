using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace SpaceInvaders.ViewModel
{
    //alltalanos parancs tipusa

    public class DelegateCommand : ICommand
    {
        private readonly Action<Object> _execute; // a tevékenységet végrehajtó lambda-kifejezés
        private readonly Func<Object, Boolean> _canExecute; // a tevékenység feltételét ellenőző lambda-kifejezés

        /// Parancs létrehozása.
        public DelegateCommand(Action<Object> execute) : this(null, execute) { }

        /// Parancs létrehozása.
        public DelegateCommand(Func<Object, Boolean> canExecute, Action<Object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;       //vegrehajtando tevekenysseg
            _canExecute = canExecute; //vegrehajthatosag feltetele
        }

        /// Végrehajthatóság változásának eseménye.
        public event EventHandler CanExecuteChanged;

        /// Végrehajthatóság ellenőrzése
        public Boolean CanExecute(Object parameter) //igaz ha vegrehajthato
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// Tevékenység végrehajtása.
        public void Execute(Object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            _execute(parameter);
        }

        /// Végrehajthatóság változásának eseménykiváltása.
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}