import { useSelector } from 'react-redux'
import { NavLink, Outlet, useNavigate } from 'react-router-dom'

const ProtectedRoute = () => {
  const { userInfo } = useSelector((state) => state.user)
  const navigate = useNavigate()

  // show unauthorized screen if no user is found in redux store
  if (!userInfo) {
    return navigate('/login')
  }

  return <Outlet />
}

export default ProtectedRoute
