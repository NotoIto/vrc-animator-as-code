using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Optional;
using Optional.Collections;

namespace notoito.vrcanimatorascode
{
    public static class UnitOption
    {
        public static Option<Unit> None() => Option.None<Unit>();
        public static Option<Unit, TException> None<TException>(TException exception) => Option.None<Unit, TException>(exception);
        public static Option<Unit> Some() => Option.Some(default(Unit));
        public static Option<Unit, TException> Some<TException>() => Option.Some<Unit, TException>(default(Unit));
        public static Option<Unit> Some(Action f) => Option.Some(new Unit(f));
        public static Option<Unit, TException> Some<TException>(Action f) => Option.Some<Unit, TException>(new Unit(f));
    }

    public static class TryUtil    {
        public static Option<TResult, Exception> Try<TResult>(Func<TResult> tryFunction)
        {
            try
            {
                return Option.Some<TResult, Exception>(tryFunction());
            }
            catch (Exception e)
            {
                return Option.None<TResult, Exception>(e);
            }
        }

        public static Option<TResult, DomainDefinedError> ToOptionSystemError<TResult>(this Option<TResult, Exception> tryOption, string message) =>
            tryOption.Match(
                some: r => Option.Some<TResult, DomainDefinedError>(r),
                none: e => Option.None<TResult, DomainDefinedError>(new SystemError($"SystemError({message})", e))
            );
    }

    public static class ErrorUtil
    {
        public static Option<TResult[], DomainDefinedError> AggregateError<TResult, TError>(
            this Option<TResult, DomainDefinedError>[] results,
            Func<string, Exception, TError> errorConstructor
            ) where TError : DomainDefinedError =>
            results.Exceptions().ToArray().Length != 0 ?
                Option.None<TResult[], DomainDefinedError>(
                    errorConstructor(
                        $"{results.Exceptions().Aggregate("", (acc, e) => $"{acc}, {e.Message}")}",
                        new Exception()
                    )
                ):
                Option.Some<TResult[], DomainDefinedError>(results.Values().ToArray());
    }
}
