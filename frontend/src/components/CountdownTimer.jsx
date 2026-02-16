import { useState, useEffect } from 'react'

const CountdownTimer = ({ startDate, endDate }) => {
  const calculateTimeLeft = () => {
    const difference = +new Date(endDate) - +new Date()
    let timeLeft = {}

    if (difference > 0) {
      timeLeft = {
        days: Math.floor(difference / (1000 * 60 * 60 * 24)),
        hours: Math.floor((difference / (1000 * 60 * 60)) % 24),
        minutes: Math.floor((difference / 1000 / 60) % 60),
        seconds: Math.floor((difference / 1000) % 60)
      }
    }

    return timeLeft
  }

  const [timeLeft, setTimeLeft] = useState(calculateTimeLeft())

  useEffect(() => {
    const timer = setTimeout(() => {
      setTimeLeft(calculateTimeLeft())
    }, 1000)

    return () => clearTimeout(timer)
  })

  const renderAnimatedDigit = (value, label) => (
    <div className='pv-animatedigit-main animate-swift-up'>
      <div className='pv-animatedigit-value '>{value}</div>
      <div className='pv-animatedigit-lable '>{label}</div>
    </div>
  );

  return (
    <>
      <div className="pv-countdowntimer-main animate-swift-up-main">
        <span className="pv-countdown-head animate-bounce-entrance">Flash Sale Start</span>
        <div className=" pv-countdowntimer-inner">
          {timeLeft.days !== undefined &&
            renderAnimatedDigit(timeLeft.days, "Days")}
          {timeLeft.hours !== undefined &&
            renderAnimatedDigit(timeLeft.hours, "Hours")}
          {timeLeft.minutes !== undefined &&
            renderAnimatedDigit(timeLeft.minutes, "Minutes")}
          {timeLeft.seconds !== undefined &&
            renderAnimatedDigit(timeLeft.seconds, "Seconds")}
        </div>
        
      </div>
    </>
  );
};

export default CountdownTimer
