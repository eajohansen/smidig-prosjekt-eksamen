import { useState, useEffect } from "react";
import { validateEmail } from "../validation";
import "bootstrap/dist/css/bootstrap.min.css";
import { sendLogin, sendRegister } from "../services/tempService";
import { useNavigate } from "react-router-dom";

const LoginPage = () => {
  const navigate = useNavigate();
  const [loginMail, setLoginMail] = useState("");
  const [loginPword, setLoginPword] = useState("");

  const [login, setLogin] = useState(2);
  const [mailCheck, setMailCheck] = useState("");
  const [pCheck, setPCheck] = useState("");
  const [confirmPword, setConfirmPword] = useState("");

  const [isHovered, setIsHovered] = useState(false);

  const [pwordErr, setPwordErr] = useState([]);

  useEffect(() => {
    setMailCheck(mailCheck);
  }, [mailCheck]);

  useEffect(() => {
    setPCheck(pCheck);
  }, [pCheck]);

  useEffect(() => {
    setLoginMail(loginMail);
  }, [loginMail]);

  useEffect(() => {
    setLoginPword(loginPword);
  }, [loginPword]);

  const handleChange = (e) => {
    const value = e.currentTarget.value;
    switch (e.currentTarget.name) {
      case "mail":
        setMailCheck(value);
        break;
      case "pword":
        setPCheck(value);
        break;
      case "cpword":
        setConfirmPword(value);
        break;
      case "loginMailName":
        setLoginMail(value);
        break;
      case "loginPwordName":
        setLoginPword(value);
        break;
    }
  };

  const handleSubmit = async () => {
    if (validateEmail(mailCheck) === true && pCheck === confirmPword) {
      console.log("frontEnd Validation successful");
      const result = await sendRegister(mailCheck, pCheck);
      if (result === true) {
        setLogin(3);
        localStorage.setItem("emailStore", mailCheck);
      } else {
        let tempArray = [];
        // for (const [key, value] of Object.entries(result)) {
        //   console.log(value[0]);
        //   tempArray.push(value[0]);
        // }
        // setPwordErr(tempArray);
      }
    } else {
      console.log("frontEnd validation unsuccessful");
    }
  };

  const handleLogin = async () => {
    const result = await sendLogin(loginMail, loginPword);
    if (result) {
      console.log("login successful");
      navigate("/");
      window.location.reload();
    }
    if (result >= 400) {
      console.log("username and password doesnt match database");
    }
  };

  const loginOrRegister = () => {
    switch (login) {
      case 1:
        return (
          <div className="loginContainer formContainer inputHover">
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
            <label>Epost</label>
            <input type="email" name="loginMailName" onChange={handleChange} />
            <label>Passord</label>
            <input
              type="password"
              name="loginPwordName"
              onChange={handleChange}
            />

            <button className="cntBtn logInBtn" onClick={handleLogin}>
              Logg Inn
            </button>
            <p className="InfoLink">
              <a href="#"> Glemt passord?</a>
            </p>
          </div>
        );
      case 2:
        return (
          <div className="loginContainer formContainer inputHover">
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
            <label>Epost</label>
            <input type="email" name="mail" onChange={handleChange} />
            <label>Passord</label>
            <input
              type="password"
              name="pword"
              className="pwordInput"
              onChange={handleChange}
              onMouseEnter={() => setIsHovered(true)}
              onMouseLeave={() => setIsHovered(false)}
            />
            <label>Gjenta Passord</label>
            <input
              type="password"
              name="cpword"
              className="pwordInput"
              onChange={handleChange}
              onMouseEnter={() => setIsHovered(true)}
              onMouseLeave={() => setIsHovered(false)}
            />
            <div className={isHovered ? "pwordDetails" : "dNone"}>
              password needs: 1 capital letter, 1 lowercase letter, 1 number, 8
              characters long
            </div>
            <div className="errorDisplay">
              <ul>
                {pwordErr.map((item, index) => (
                  <li key={index}>{item}</li>
                ))}
              </ul>
            </div>
            <p className="InfoLink">
              Ved å klikke fortsett godtar du våre <a href="#">brukervilkår</a>{" "}
              og <br />
              <a href="#">personvernserklæring</a>
            </p>
            <button
              className="cntBtn"
              onClick={() => {
                handleSubmit();
              }}
            >
              Fortsett
            </button>
          </div>
        );
      case 3:
        // return <ProfileForm userEmail={mailCheck} />;
        navigate("/register");
    }
  };
  return loginOrRegister();
};

export default LoginPage;
