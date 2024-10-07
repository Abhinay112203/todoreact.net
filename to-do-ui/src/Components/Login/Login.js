import { useLoading } from "../Loader/LoadingProvider";
import "./Login.css";
function Login() {
  const { loading, setLoading } = useLoading();
  function handleClick() {
    setLoading(!loading);
  }
  return <div className="container"> Login Page</div>;
}

export default Login;
