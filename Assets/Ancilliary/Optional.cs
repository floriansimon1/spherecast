namespace Optional {
  public class UnwrapNoneError: System.Exception {}

  public abstract class Optional<T> {
    public readonly bool valuePresent;

    public bool valueAbsent {
      get {
        return !valuePresent;
      }
    }

    protected Optional(bool valuePresent) {
      this.valuePresent = valuePresent;
    }

    public abstract T get();
  }

  public class Some<T>: Optional<T> {
    private T value;

    public Some(T value): base(true) {
      this.value = value;
    }

    public override T get() {
      return value;
    }
  }

  public class None<T>: Optional<T> {
    public None(): base(false) {}

    public override T get() {
      throw new UnwrapNoneError();
    }
  }
}