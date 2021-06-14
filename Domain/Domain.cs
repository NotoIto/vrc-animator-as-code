using System;

namespace notoito.vrcanimatorascode
{
    public class Unit { 
        public Unit()
        {

        }
    
        public Unit(Action f)
        {
            f();
        }
    }

    public class DomainDefinedError : Exception {
        public DomainDefinedError(string message, Exception e = null) : base(message, e) { }
    }

    public class SystemError : DomainDefinedError {
        public SystemError(string message, Exception e = null) : base($"SystemError({message})", e) { }
    }

    public class ValidationError : DomainDefinedError {
        public ValidationError(string message, Exception e = null) : base($"ValidationError({message})", e) { }
    }
}
