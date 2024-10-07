import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./Components/Home/Home";
import LoadingComponent from "./Components/Loader/Loader";
import { LoadingProvider } from "./Components/Loader/LoadingProvider";
import Login from "./Components/Login/Login";
import AuthorisedLayout from "./Layouts/Authorised";
import UnAuthorisedLayout from "./Layouts/UnAuthorised";
function WildRoute() {
  return <Navigate to="/login" />;
}

function App() {
  return (
    <LoadingProvider>
      <LoadingComponent />
      <BrowserRouter>
        <Routes>
          <Route path="/auth" element={<AuthorisedLayout />}>
            <Route index element={<Home />} />
          </Route>
          <Route path="/" element={<UnAuthorisedLayout />}>
            <Route index element={<Login />} />
            <Route path="login" element={<Login />} />
          </Route>
          <Route path="*" index element={<WildRoute />} />
        </Routes>
      </BrowserRouter>
    </LoadingProvider>
  );
}

export default App;
