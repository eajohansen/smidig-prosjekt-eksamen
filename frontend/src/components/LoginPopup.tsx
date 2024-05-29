import { useState } from "react";
import { validateEmail } from "../validateEmail";
const LoginPopup = () => {
  const [login, setLogin] = useState(true);
  const [mailCheck, setMailCheck] = useState("");

  const loginOrRegister = () => {
    if (login) {
      return (
        <div className="loginContainer">
          <h2>Logg inn</h2>
          <div className="loginBtnContainer">
            <button className="loginToggleBtn">Logg Inn</button>
            <button
              className="registerToggleBtn"
              onClick={() => setLogin(false)}
            >
              Opprett Profil
            </button>
          </div>
          <label style={{ alignSelf: "flex-start" }}>Epost</label>
          <input
            type="email"
            value={mailCheck}
            onChange={() => setMailCheck(value)}
          />
          <label style={{ alignSelf: "flex-start" }}>Passord</label>
          <input type="password" />

          <button>Logg Inn</button>
          <button onClick={() => {}}>Validate email</button>
        </div>
      );
    } else if (!login) {
      return (
        <div className="loginContainer">
          <h2>Opprett Profil</h2>
          <div className="loginBtnContainer">
            <button className="loginToggleBtn" onClick={() => setLogin(true)}>
              Logg inn
            </button>
            <button className="registerToggleBtn">Opprett Profil</button>
          </div>
          <label style={{ alignSelf: "flex-start" }}>Epost</label>
          <input type="email" />
          <label style={{ alignSelf: "flex-start" }}>Passord</label>
          <input type="password" />
          <label style={{ alignSelf: "flex-start" }}>Gjenta Passord</label>
          <input type="password" />

          <button>Fortsett</button>
        </div>
      );
    }
  };
  return loginOrRegister();
};

export default LoginPopup;
