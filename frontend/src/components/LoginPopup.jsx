import { useState, useEffect, SyntheticEvent } from "react";
import { validateEmail, validatePword } from "../validation";
import { ProfileForm } from "./ProfileForm";
import "bootstrap/dist/css/bootstrap.min.css";
const LoginPopup = () => {
  const [login, setLogin] = useState(2);
  const [mailCheck, setMailCheck] = useState("");
  const [pCheck, setPCheck] = useState("");

  useEffect(() => {
    setMailCheck(mailCheck);
  }, [mailCheck]);

  useEffect(() => {
    setPCheck(pCheck);
  }, [pCheck]);

  const handleChange = (e) => {
    setMailCheck(e.currentTarget.value);
  };

  const handlePChange = (e) => {
    setPCheck(e.currentTarget.value);
  };
  const loginOrRegister = () => {
    switch (login) {
      case 1:
        return (
          <div className="loginContainer formContainer">
            <h2>Logg inn</h2>
            <div className="loginBtnContainer">
              <button
                className="loginToggleBtn"
                style={{ color: "white", backgroundColor: "#497D95" }}
              >
                Logg Inn
              </button>
              <button className="registerToggleBtn" onClick={() => setLogin(2)}>
                Opprett Profil
              </button>
            </div>
            <label style={{ alignSelf: "flex-start" }}>Epost</label>
            <input type="email" onChange={handleChange} />
            <label style={{ alignSelf: "flex-start" }}>Passord</label>
            <input type="password" />

            <button className="cntBtn">Logg Inn</button>
            <p className="InfoLink"><a  href="#"> Glemt passord?</a></p>
          </div>
        );
      case 2:
        return (
          <div className="loginContainer formContainer">
            <h2>Opprett Profil</h2>
            <div className="loginBtnContainer">
              <button className="loginToggleBtn" onClick={() => setLogin(1)}>
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
            <input type="email" onChange={handleChange} />
            <label style={{ alignSelf: "flex-start" }}>Passord</label>
            <input type="password" />
            <label style={{ alignSelf: "flex-start" }}>Gjenta Passord</label>
            <input type="password" />

            <button
              className="cntBtn"
              onClick={() => {
                console.log(validateEmail(mailCheck));
                console.log(mailCheck);
                if (validateEmail(mailCheck) == true) {
                  setLogin(3);
                }
              }}
            >
              Fortsett
            </button>
            <p className="InfoLink">Ved å klikke fortsett godtar du våre <a href="#">brukervilkår</a> og <br/><a href="#">personvernserklæring</a></p>
          </div>
        );
      case 3:
        return <ProfileForm userEmail={mailCheck} />;
    }
  };
  return loginOrRegister();
};

export default LoginPopup;
