import { Link, useNavigate } from "react-router-dom";
import "bootstrap-icons/font/bootstrap-icons.css";
import { axiosInstance } from "../../services/helpers.js";
export const MainNav = ({ version }) => {
  const navigate = useNavigate();
  const handleLogout = async () => {
    localStorage.clear();
    navigate("login");
    axiosInstance.defaults.headers.common["Authorization"] = "";
    window.location.reload();
  };
  switch (version) {
    case "user":
      return (
        <header>
          <div className="navHeader">
            <Link to="/" className="navItem">
              <i className="bi bi-house"></i> <p>Hjem</p>
            </Link>

            <Link to="profile" className="navItem">
              <i className="bi bi-person"></i> <p>Min Profil</p>
            </Link>

            <Link to="tickets" className="navItem">
              <i className="bi bi-ticket"></i> <p>Biletter</p>
            </Link>

            <Link to="events" className="navItem">
              <i className="bi bi-calendar-event"></i> <p>Arrangementer</p>
            </Link>

            <button onClick={handleLogout} className="navItem">
              <i className="bi bi-box-arrow-left"></i> <p>Logg ut</p>
            </button>
          </div>
        </header>
      );
    case "organizer":
      return (
        <header>
          <div className="navHeader">
            <Link to="/" className="navItem">
              <i className="bi bi-house"></i> <p>Hjem</p>
            </Link>
            <Link to="orgProfile" className="navItem">
              <i className="bi bi-person"></i> <p>Org Profil</p>
            </Link>
            <Link to="events" className="navItem">
              <i className="bi bi-calendar-event"></i> <p>Arrangementer</p>
            </Link>
            <button onClick={handleLogout} className="navItem">
              <i className="bi bi-box-arrow-left"></i> <p>Logg ut</p>
            </button>
          </div>
        </header>
      );
    default:
      return (
        <header>
          <div className="navHeader">
            <Link to="login" className="navItem">
              <i className="bi bi-box-arrow-right"></i> <p>Logg inn</p>
            </Link>
          </div>
        </header>
      );
  }
};
