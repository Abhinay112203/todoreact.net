import { useLoading } from "./LoadingProvider";
import './Loader.css';


function LoadingComponent(){
  const { loading, setLoading } = useLoading();
    return <div className={loading? "showLoader" : "hideLoader"} > Loading </div>
}

export default LoadingComponent;