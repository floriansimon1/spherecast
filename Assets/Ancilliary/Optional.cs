namespace Optional {
  public class UnwrapNoneError: System.Exception {}

  public delegate void OnValue<T>(T value);

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
    public abstract void tap(OnValue<T> onValue);
  }

  public class Some<T>: Optional<T> {
    private T value;

    public Some(T value): base(true) {
      this.value = value;
    }

    public override T get() {
      return value;
    }

    public override void tap(OnValue<T> onValue) {
      onValue(value);
    }
  }

  public class None<T>: Optional<T> {
    public None(): base(false) {}

    public override T get() {
      throw new UnwrapNoneError();
    }

    public override void tap(OnValue<T> onValue) {
    }
  }
}