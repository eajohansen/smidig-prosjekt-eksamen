import { Link } from "react-router-dom";

export const MainNav = ({ version }) => {
  switch (version) {
    case "user":
      return (
        <header>
          <div className="navHeader">
            <Link to="/" className="navItem">
              Hjem / TEST
            </Link>

            <Link to="profile" className="navItem">
              Min Profil
            </Link>

            <Link to="tickets" className="navItem">
              Biletter
            </Link>

            <Link to="events" className="navItem">
              Arrangementer
            </Link>
          </div>
        </header>
      );
    case "organizer":
      return (
        <header>
          <div>whatever</div>
        </header>
      );
    case "dummy":
      return (
        <header>
          <div>whatever</div>
        </header>
      );
  }
};
