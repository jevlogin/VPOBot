using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Timers;
using Timer = System.Timers.Timer;


namespace WORLDGAMEDEVELOPMENT
{
    public class ProgressUsers
    {
        #region Fields

        public event Action<ProgressUsers>? ProgressUpdated;

        private DateTime _dateTimeOfTheNextStep;
        private DateTime _dateNextDayVPO;
        private UpdateState _updateState = UpdateState.None;
        private int _currentDay;
        private int _currentStep;
        private bool _isTheNextStepSheduledInTime;

        private Timer _timerEvent;
        private Timer _timerNextDay;

        #endregion


        #region Properties

        [Key]
        public long UserId { get; set; }

        public UpdateState UpdateState
        {
            get
            {
                return _updateState;
            }
            set
            {
                _updateState = value;
                switch (_updateState)
                {
                    case UpdateState.None:
                        Console.WriteLine($"Нет обновлений - UpdateState.None");
                        ProgressUpdated?.Invoke(this);

                        break;
                    case UpdateState.FullUpdate:
                        Console.WriteLine($"Запускаем полное обновление");
                        ProgressUpdated?.Invoke(this);

                        break;
                    case UpdateState.UpdateDate:
                        Console.WriteLine($"Запускаем обновление даты и шагов выполнения");
                        StartCheckingDailyProgressUpdates();
                        StartCheckingForUpdatesOfProgressSteps();
                        ProgressUpdated?.Invoke(this);

                        break;
                }
            }
        }

        [Column]
        public DateTime DateNextDayVPO
        {
            get => _dateNextDayVPO;
            set
            {
                _dateNextDayVPO = value;
                if (DateTime.Today < _dateNextDayVPO)
                {
                    if (_timerNextDay != null)
                    {
                        TimerNextDayDispose();
                    }
                    UpdateState = UpdateState.UpdateDate;
                }
            }
        }

        [Column]
        public DateTime DateTimeOfTheNextStep
        {
            get => _dateTimeOfTheNextStep;
            set
            {
                _dateTimeOfTheNextStep = value;
                if (DateTime.Today < _dateTimeOfTheNextStep)
                {
                    IsTheNextStepSheduledInTime = false;
                    if (_timerEvent != null)
                    {
                        TimerNextStepDispose();
                    }
                    UpdateState = UpdateState.UpdateDate;
                }
            }
        }

        public bool IsTheNextStepSheduledInTime
        {
            get => _isTheNextStepSheduledInTime;
            set
            {
                _isTheNextStepSheduledInTime = value;
            }
        }

        public int CurrentDay
        {
            get => _currentDay;
            set
            {
                _currentDay = value;
            }
        }


        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                _currentStep = value;
                IsTheNextStepSheduledInTime = true;
                UpdateState = UpdateState.FullUpdate;
            }
        }

        #endregion


        #region ClassLifeCycles

        [JsonConstructor]
        public ProgressUsers(long userId, int currentDay, int currentStep, DateTime dateTimeOfTheNextStep, DateTime dateNextDayVPO,
            bool isTheNextStepSheduledInTime, UpdateState updateState)
        {
            UserId = userId;
            _currentDay = currentDay;
            _currentStep = currentStep;
            _dateTimeOfTheNextStep = dateTimeOfTheNextStep;
            _dateNextDayVPO = dateNextDayVPO;
            _isTheNextStepSheduledInTime = isTheNextStepSheduledInTime;
            _updateState = updateState;
        }


        #endregion


        #region ProgressStep

        private void StartCheckingForUpdatesOfProgressSteps()
        {
            if (DateTime.UtcNow.ToLocalTime() < DateTimeOfTheNextStep && !IsTheNextStepSheduledInTime)
            {
                if (_timerEvent == null)
                {
                    Console.WriteLine($"DateTimeOfTheNextStep == {DateTimeOfTheNextStep}");
                    _timerEvent = new Timer
                    {
                        Interval = (DateTimeOfTheNextStep - DateTime.UtcNow.ToLocalTime()).TotalMilliseconds,
                        AutoReset = false,
                    };
                    _timerEvent.Elapsed += CheckSheduledEvent;
                    _timerEvent.Start();
                }
            }
            else if (!IsTheNextStepSheduledInTime && _timerEvent == null)
            {
                Console.WriteLine($"Текущее время больше > Времени следующего шага. Но следующий шаг, не был выполнен.");
                _timerEvent = new Timer
                {
                    Interval = 10000,
                    AutoReset = false,
                };
                _timerEvent.Elapsed += CheckSheduledEvent;
                _timerEvent.Start();
            }
        }

        private void CheckSheduledEvent(object? sender, ElapsedEventArgs e)
        {
            IsTheNextStepSheduledInTime = true;

            CurrentStep++;
            if(_timerEvent != null)
            {
                TimerNextStepDispose();
            }
        }

        private void TimerNextStepDispose()
        {
            _timerEvent.Elapsed -= CheckSheduledEvent;
        }

        #endregion


        #region NextDay

        private void StartCheckingDailyProgressUpdates()
        {
            if (DateTime.UtcNow.ToLocalTime() < DateNextDayVPO)
            {
                _timerNextDay = new Timer
                {
                    Interval = (DateNextDayVPO - DateTime.UtcNow.ToLocalTime()).TotalMilliseconds,
                    AutoReset = false
                };
                _timerNextDay.Elapsed += CheckEventNextDay;
                _timerNextDay.Start();
            }
        }

        private void CheckEventNextDay(object? sender, ElapsedEventArgs e)
        {
            _currentStep = 1;
            CurrentStep++;
            if (_timerNextDay != null)
            {
                TimerNextDayDispose();
            }
        }

        private void TimerNextDayDispose()
        {
            _timerNextDay.Elapsed -= CheckEventNextDay;
        }
        #endregion

    }
}