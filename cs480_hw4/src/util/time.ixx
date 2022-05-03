//
// Created by Gabriel Lugo on 3/27/2021.
// Flugel Framework: https://github.com/GTLugo/flugel_framework
//

export module time;

#pragma warning(suppress:5050)
import std.core;

// duration types
export using NanoSeconds = std::chrono::duration<double, std::nano>;
export using MicroSeconds = std::chrono::duration<double, std::micro>;
export using MilliSeconds = std::chrono::duration<double, std::milli>;
export using Seconds = std::chrono::duration<double>;
export using Minutes = std::chrono::duration<double, std::ratio<60>>;
export using Hours = std::chrono::duration<double, std::ratio<3600>>;
// clock types
export using ClockSystem = std::chrono::system_clock;
export using ClockSteady = std::chrono::steady_clock;
export using ClockAccurate = std::chrono::high_resolution_clock;
// time point
export using TimePoint = ClockSteady::time_point;
export using TimePointAccurate = ClockAccurate::time_point;
export using TimePointSystem = ClockSystem::time_point;

export class Stopwatch {
public:
  Stopwatch() {
    start();
  }

  explicit Stopwatch(TimePoint timePoint) {
    start(timePoint);
  }

  void start() { start(ClockSteady::now()); }

  void start(TimePoint timePoint) {
    start_ = timePoint;
  }

  template<class D>
  [[nodiscard]] double startTime() const {
    return D((start_).time_since_epoch()).count();
  }

  template<class D>
  [[nodiscard]] double getTimeElapsed() const {
    return D((ClockSteady::now() - start_)).count();
  }
private:
  TimePoint start_{};
};

export class Time {
public:
  static void init(double tickRate = 128., std::uint32_t bailCount = 1024U) {
    // This is awful and messy, but it'll prevent anyone outside the App class
    // from reinitializing Time, which would cause the engine, the app, life,
    // the universe, and all catgirls to die.
    if (!virgin_) return;
    std::cout << "Initializing Time...\n";

    tickRate_ = tickRate;
    bailCount_ = bailCount;
    gameLast_ = TimePoint{ ClockSteady::now() };
    gameCurrent_ = TimePoint{ ClockSteady::now() };
    fixedTimeStep_ = Seconds{ 1. / tickRate_ };
  }
  //    explicit Time(double tickRate, uint32_t bailCount = 1024U)
  //      : tickRate_{tickRate},
  //        bailCount_{bailCount},
  //        stopwatch_{ClockSteady::now()},
  //        gameLast_{ClockSteady::now()},
  //        gameCurrent_{ClockSteady::now()} {
  //      fixedTimeStep_ = Seconds{1. / tickRate_};
  //    }
  //    ~Time() = default;

  [[nodiscard]] static double tickRate() {
    return tickRate_;
  }

  template<class Duration>
  [[nodiscard]] static double fixedStep() {
    return Duration::duration((fixedTimeStep_)).count();
  }

  template<class Duration>
  [[nodiscard]] static double start() {
    return stopwatch().startTime<Duration>();
  }

  template<class Duration>
  [[nodiscard]] static double sinceStart() {
    return stopwatch().getTimeElapsed<Duration>();
  }

  template<typename Duration>
  [[nodiscard]] static double now() {
    return Duration::duration(ClockSteady::now().time_since_epoch()).count();
  }

  template<class Duration>
  [[nodiscard]] static double delta() {
    return Duration::duration((delta_)).count();
  }

  template<class Duration>
  [[nodiscard]] static double lag() {
    return Duration::duration((lag_)).count();
  }

  static void update() {
    // FLUGEL_ENGINE_TRACE("Update!");
    gameCurrent_ = ClockSteady::now();
    // Seconds::duration()
    delta_ = Seconds::duration((gameCurrent_ - gameLast_).count());
    gameLast_ = gameCurrent_;
    lag_ += delta_;
    stepCount_ = 0U;
  }

  static void tick() {
    // FLUGEL_ENGINE_TRACE("Tick!");
    lag_ -= fixedTimeStep_;
    ++stepCount_;
  }

  [[nodiscard]] static bool shouldDoTick() {
    if (stepCount_ >= bailCount_) {
      std::cerr << "Struggling to catch up with physics rate!\n";
    }

    return lag_.count() >= fixedTimeStep_.count() && stepCount_ < bailCount_;
  }
private:
  static inline bool virgin_{ true };
  // fixed number of ticks per second. this will be used for physics and anything else in fixed update
  static inline double tickRate_{};
  static inline Seconds fixedTimeStep_{};
  // bail out of the fixed updates if iterations exceeds this amount to prevent lockups
  // on extremely slow systems where updateFixed may be longer than fixedTimeStep_
  static inline std::uint32_t bailCount_{};

  static inline TimePoint gameLast_{}; // when last frame started
  static inline TimePoint gameCurrent_{}; // when this frame started
  static inline Seconds delta_{ Seconds{1. / 60.} }; // how much time last frame took
  static inline Seconds lag_{ Seconds::zero() }; // how far behind the game is from real world
  static inline std::uint32_t stepCount_{ 0U };

  static const Stopwatch& stopwatch() {
    static const Stopwatch sw{ ClockSteady::now() };
    return sw;
  };
};