﻿using jim.wiki.core.Errors;
using jim.wiki.core.Extensions;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;


namespace jim.wiki.core.Results
{
    public class Result
    {
        public bool Success => !Errors.ContainElements(); 
        public bool IsFailed => !Success;
        public bool HasValidationError => Validations.ContainElements();

        [JsonIgnore]
        public CancellationToken CancellationToken { get; set; }

        private IList<Error> _errors;
        public ReadOnlyCollection<Error> Errors  => _errors.AsReadOnly();

        private IList<Error> _validations;
        public ReadOnlyCollection<Error> Validations => _validations.AsReadOnly();

        internal Result(CancellationToken cancellationToken = default)
        {
            _errors = new List<Error>();
            _validations = new List<Error>();
            CancellationToken = cancellationToken;
        }

        internal Result(IEnumerable<Error> errors, CancellationToken cancellationToken = default):this(cancellationToken)
        {
            
            AddError(errors.ToArray());
        }

        public void AddError(params Error[] errors)
        {
            _errors = _errors.Concat(errors).ToList();
        }

        public void AddValidations(params Error[] validations)
        {
            _validations = _validations.Concat(validations).ToList();
        }

        public static Result Ok(CancellationToken cancellationToken = default) => new Result(cancellationToken);
        public static Result<T> Ok<T>(T value, CancellationToken cancellationToken = default) => new Result<T>(value, cancellationToken);
        public static Result Fail(IEnumerable<Error> errors, CancellationToken cancellationToken = default) => new Result(errors, cancellationToken);
        public static Result Fail(Error error, CancellationToken cancellationToken = default) => new Result(new Error[] { error }, cancellationToken);
        public static Result<T> Fail<T>(Error error, CancellationToken cancellationToken = default) => new Result<T>(new Error[] { error }, cancellationToken);
        public static Result<T> Fail<T>(IEnumerable<Error> errors, CancellationToken cancellationToken = default) => new Result<T>(errors, cancellationToken);

    }

    public class Result<T> : Result
    {
        public T Value { get; set; }

        internal Result(T value,CancellationToken cancellationToken = default) : base(cancellationToken)
        {
            Value = value;
        }

        internal Result(IEnumerable<Error> errors,CancellationToken cancellationToken = default) : base(errors,cancellationToken)
        {
            Value = default;
        }

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(value);
        }

    }

    public class ResultSearch<T>: Result<IEnumerable<T>>
    {
        public int Total { get; set; }
        public int? Page { get; set; }
        public int? SizePage { get; set; }
        public ResultSearch(IEnumerable<T> value, int total, int? page, int? sizePage):base(value)
        {
            Total = total;
            Page = page;
            SizePage = sizePage;
        }
    }
}
