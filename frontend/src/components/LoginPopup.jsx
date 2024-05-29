import { useState, useEffect, SyntheticEvent } from "react";
import { validateEmail } from "../validateEmail";
import "bootstrap/dist/css/bootstrap.min.css";
const LoginPopup = () => {
  const [login, setLogin] = useState(true);
  const [mailCheck, setMailCheck] = useState("");

  useEffect(() => {
    setMailCheck(mailCheck);
  }, [mailCheck]);

  const handleChange = (e) => {
    setMailCheck(e.currentTarget.value);
  };
  const loginOrRegister = () => {
    if (login) {
      return (
        <div className="loginContainer">
          <h2>Logg inn</h2>
          <div className="loginBtnContainer">
            <button
              className="loginToggleBtn"
              style={{ color: "white", backgroundColor: "#497D95" }}
            >
              Logg Inn
            </button>
            <button
              className="registerToggleBtn"
              onClick={() => setLogin(false)}
            >
              Opprett Profil
            </button>
          </div>
          <label style={{ alignSelf: "flex-start" }}>Epost</label>
          <input type="email" onChange={handleChange} />
          <label style={{ alignSelf: "flex-start" }}>Passord</label>
          <input type="password" />

          <button className="cntBtn">Logg Inn</button>
          <button
            onClick={() => {
              console.log(validateEmail(mailCheck));
            }}
          >
            Validate email
          </button>
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
            <button
              className="registerToggleBtn"
              style={{ color: "white", backgroundColor: "#497D95" }}
            >
              Opprett Profil
            </button>
          </div>
          <label style={{ alignSelf: "flex-start" }}>Epost</label>
          <input type="email" />
          <label style={{ alignSelf: "flex-start" }}>Passord</label>
          <input type="password" />
          <label style={{ alignSelf: "flex-start" }}>Gjenta Passord</label>
          <input type="password" />

          <button className="cntBtn">Fortsett</button>
        </div>
      );
    }
  };
  return loginOrRegister();
};

export default LoginPopup;
