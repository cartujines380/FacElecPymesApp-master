﻿using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Behaviors.Base;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Behaviors
{
    public class EventToCommandBehavior : BindableBehavior<View>
    {
        public static BindableProperty EventNameProperty = BindableProperty.CreateAttached(
            "EventName",
            typeof(string),
            typeof(EventToCommandBehavior),
            null,
            BindingMode.OneWay
        );

        public static BindableProperty CommandProperty = BindableProperty.CreateAttached(
            "Command",
            typeof(ICommand),
            typeof(EventToCommandBehavior),
            null,
            BindingMode.OneWay
        );

        public static BindableProperty CommandParameterProperty = BindableProperty.CreateAttached(
            "CommandParameter",
            typeof(object),
            typeof(EventToCommandBehavior),
            null,
            BindingMode.OneWay
        );

        public static BindableProperty EventArgsConverterProperty = BindableProperty.CreateAttached(
            "EventArgsConverter",
            typeof(IValueConverter),
            typeof(EventToCommandBehavior),
            null,
            BindingMode.OneWay
        );

        public static BindableProperty EventArgsConverterParameterProperty = BindableProperty.CreateAttached(
            "EventArgsConverterParameter",
            typeof(object),
            typeof(EventToCommandBehavior),
            null,
            BindingMode.OneWay
        );

        protected Delegate m_handler;
        private EventInfo m_eventInfo;

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter EventArgsConverter
        {
            get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
            set { SetValue(EventArgsConverterProperty, value); }
        }

        public object EventArgsConverterParameter
        {
            get { return GetValue(EventArgsConverterParameterProperty); }
            set { SetValue(EventArgsConverterParameterProperty, value); }
        }

        protected override void OnAttachedTo(View visualElement)
        {
            base.OnAttachedTo(visualElement);

            var events = AssociatedObject.GetType().GetRuntimeEvents().ToArray();

            if (events.Any())
            {
                m_eventInfo = events.FirstOrDefault(e => e.Name == EventName);

                if (m_eventInfo == null)
                    throw new ArgumentException(string.Format("EventToCommand: Can't find any event named '{0}' on attached type", EventName));

                AddEventHandler(m_eventInfo, AssociatedObject, OnFired);
            }
        }

        protected override void OnDetachingFrom(View view)
        {
            if (m_handler != null)
                m_eventInfo.RemoveEventHandler(AssociatedObject, m_handler);

            base.OnDetachingFrom(view);
        }

        private void AddEventHandler(EventInfo eventInfo, object item, Action<object, EventArgs> action)
        {
            var eventParameters = eventInfo.EventHandlerType
                .GetRuntimeMethods().First(m => m.Name == "Invoke")
                .GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType))
                .ToArray();

            var actionInvoke = action.GetType()
                .GetRuntimeMethods().First(m => m.Name == "Invoke");

            m_handler = Expression.Lambda(
                eventInfo.EventHandlerType,
                Expression.Call(Expression.Constant(action), actionInvoke, eventParameters[0], eventParameters[1]),
                eventParameters
            )
            .Compile();

            eventInfo.AddEventHandler(item, m_handler);
        }

        private void OnFired(object sender, EventArgs eventArgs)
        {
            if (Command == null)
                return;

            var parameter = CommandParameter;

            if (eventArgs != null && eventArgs != EventArgs.Empty)
            {
                parameter = eventArgs;

                if (EventArgsConverter != null)
                {
                    parameter = EventArgsConverter.Convert(eventArgs, typeof(object), EventArgsConverterParameter, CultureInfo.CurrentUICulture);
                }
            }

            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }
    }
}
