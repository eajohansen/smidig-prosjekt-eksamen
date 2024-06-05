import { useState, useEffect, SyntheticEvent } from "react";
import { validateEmail, validatePword } from "../validation";
import { ProfileForm } from "./ProfileForm";
import "bootstrap/dist/css/bootstrap.min.css";
import { sendRegister } from "../services/tempService";
const LoginPopup = () => {
  const [login, setLogin] = useState(2);
  const [mailCheck, setMailCheck] = useState("");
  const [pCheck, setPCheck] = useState("");
  const [isHovered, setIsHovered] = useState(false);

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
  const handleRegister = async () => {
    const result = await sendRegister(mailCheck, pCheck);
    if (result === "Success!") {
      setLogin(3);
    } else {
      for (const [key, value] of Object.entries(result)) {
        console.log(value[0]);
      }
    }
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
            <input
              type="password"
              className="pwordInput"
              onChange={handlePChange}
              onMouseEnter={() => setIsHovered(true)}
              onMouseLeave={() => setIsHovered(false)}
            />
            <label style={{ alignSelf: "flex-start" }}>Gjenta Passord</label>
            <input
              type="password"
              className="pwordInput"
              onMouseEnter={() => setIsHovered(true)}
              onMouseLeave={() => setIsHovered(false)}
            />
            <div className={isHovered ? "pwordDetails" : "dNone"}>
              password needs: 1 capital letter, 1 lowercase letter, 1 number, 8
              characters long
            </div>
            <button
              className="cntBtn"
              onClick={() => {
                console.log(validateEmail(mailCheck));
                console.log(mailCheck);
                if (validateEmail(mailCheck) == true) {
                  handleRegister();
                }
              }}
            >
              Fortsett
            </button>
          </div>
        );
      case 3:
        return <ProfileForm userEmail={mailCheck} />;
    }
  };
  return loginOrRegister();
};

export default LoginPopup;
