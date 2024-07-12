using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.core.Results
{
    public static class ResultExtensions
    {    {
        
        public static Result Then(this Result result, Func<CancellationToken, Result> func)
        {
            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return func(result.CancellationToken);

        }

        public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, CancellationToken, Result<TOut>> func)
        {
            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return func(result.Value, result.CancellationToken);
        }

        public static Result<TOut> Then<TOut>(this Result result, Func<CancellationToken, Result<TOut>> func)
        {
            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return func(result.CancellationToken);
        }

        public static Result Then<TIn>(this Result<TIn> result, Func<TIn, CancellationToken, Result> func)
        {
            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return func(result.Value, result.CancellationToken);
        }

        public static Result Then(this Result result, Func<Result> func)
        {
            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return func();

        }

        public static Result<TOut> Then<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
        {
            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return func(result.Value);
        }

        public static Result<TOut> Then<TOut>(this Result result, Func<Result<TOut>> func)
        {
            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return func();
        }

        public static Result Then<TIn>(this Result<TIn> result, Func<TIn, Result> func)
        {
            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return func(result.Value);
        }



        public async static Task<Result> ThenAsync(this Task<Result> source, Func<CancellationToken, Task<Result>> func)
        {
            var result = await source;

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return await func(result.CancellationToken);

        }

        public async static Task<Result<TOut>> ThenAsync<TIn, TOut>(this Task<Result<TIn>> source, Func<TIn, CancellationToken, Task<Result<TOut>>> func)
        {

            var result = await source;

            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return await func(result.Value, result.CancellationToken);
        }

        public async static Task<Result<TOut>> ThenAsync<TOut>(this Task<Result> source, Func<CancellationToken, Task<Result<TOut>>> func)
        {
            var result = await source;

            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return await func(result.CancellationToken);
        }

        public async static Task<Result> ThenAsync<TIn>(this Task<Result<TIn>> source, Func<TIn, CancellationToken, Task<Result>> func)
        {
            var result = await source;

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return await func(result.Value, result.CancellationToken);
        }


        public async static Task<Result> ThenAsync(this Task<Result> source, Func<Task<Result>> func)
        {
            var result = await source;

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return await func();

        }

        public async static Task<Result<TOut>> ThenAsync<TIn, TOut>(this Task<Result<TIn>> source, Func<TIn, Task<Result<TOut>>> func)
        {
            var result = await source;

            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return await func(result.Value);
        }

        public async static Task<Result<TOut>> ThenAsync<TOut>(this Task<Result> source, Func<Task<Result<TOut>>> func)
        {
            var result = await source;

            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return await func();
        }

        public async static Task<Result> ThenAsync<TIn>(this Task<Result<TIn>> source, Func<TIn, Task<Result>> func)
        {
            var result = await source;

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return await func(result.Value);
        }


        public async static Task<Result> ThenAsync(this Result result, Func<CancellationToken, Task<Result>> func)
        {


            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return await func(result.CancellationToken);

        }

        public async static Task<Result<TOut>> ThenAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, CancellationToken, Task<Result<TOut>>> func)
        {



            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return await func(result.Value, result.CancellationToken);
        }

        public async static Task<Result<TOut>> ThenAsync<TOut>(this Result result, Func<CancellationToken, Task<Result<TOut>>> func)
        {


            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return await func(result.CancellationToken);
        }

        public async static Task<Result> ThenAsync<TIn>(this Result<TIn> result, Func<TIn, CancellationToken, Task<Result>> func)
        {

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return await func(result.Value, result.CancellationToken);
        }


        public async static Task<Result> ThenAsync(this Result result, Func<Task<Result>> func)
        {


            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return await func();

        }

        public async static Task<Result<TOut>> ThenAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func)
        {


            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors, result.CancellationToken);
            }

            return await func(result.Value);
        }

        public async static Task<Result<TOut>> ThenAsync<TOut>(this Result result, Func<Task<Result<TOut>>> func)
        {


            if (result.IsFailed)
            {
                return Result.Fail<TOut>(result.Errors.ToArray(), result.CancellationToken);
            }

            return await func();
        }

        public async static Task<Result> ThenAsync<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func)
        {


            if (result.IsFailed)
            {
                return Result.Fail(result.Errors, result.CancellationToken);
            }

            return await func(result.Value);
        }
    }
}
