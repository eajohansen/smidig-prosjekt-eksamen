import { Link } from "react-router-dom";
import "bootstrap-icons/font/bootstrap-icons.css";
export const MainNav = ({ version }) => {
  console.log(version);
  switch (version) {
    case "user":
      return (
        <header>
          <div className="navHeader">
            <Link to="/" className="navItem">
              <i className="bi bi-house"></i> <p>Hjem / TEST</p>
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

            <Link to="/" className="navItem">
              <i className="bi bi-box-arrow-left"></i> <p>Logg ut</p>
            </Link>
          </div>
        </header>
      );
    case "organizer":
      return (
        <header>
          {console.log("organizer")}
          <div className="navHeader">
            <Link to="/" className="navItem">
              <i className="bi bi-house"></i> <p>Hjem / TEST</p>
            </Link>

            <Link to="profile" className="navItem">
              <i className="bi bi-person"></i> <p>Min Profil</p>
            </Link>

            <Link to="events" className="navItem">
              <i className="bi bi-calendar-event"></i> <p>Arrangementer</p>
            </Link>
            <Link to="/" className="navItem">
              <i className="bi bi-box-arrow-left"></i> <p>Logg ut</p>
            </Link>
            <Link to="createevent" className="navItem">
              <i className="bi bi-calendar-event"></i> <p>Lag Arrangementer</p>
            </Link>
            <Link to="orgProfile" className="navItem">
              <i className="bi bi-person"></i> <p>Profil</p>
            </Link>
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
