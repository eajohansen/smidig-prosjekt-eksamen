import { useState, useEffect, SyntheticEvent } from "react";
import "bootstrap-icons/font/bootstrap-icons.css";
export const ProfileForm = ({ userEmail }) => {
  const [allergies, setAllergies] = useState([]);
  const [newAllergy, setNewAllergy] = useState("");
  useEffect(() => {
    setNewAllergy(newAllergy);
  }, [newAllergy]);

  const handleChange = (e) => {
    setNewAllergy(e.currentTarget.value);
  };

  const handleClick = () => {
    if (allergies.length > 5) {
      alert("Beklager! Du kan ikke legge til fler enn 6 allergier.");
    } else {
      setAllergies([...allergies, newAllergy]);
      setNewAllergy("");
    }
  };
  return (
    <div className="profileInfoContainer formContainer">
      <div className="userInfoContainer">
        <h2>Opprett Bruker</h2>
        <hr></hr>
        <label htmlFor="emailRegister">Epost</label>
        <div className="emailContainer">
          <input type="email" id="emailRegister" value={userEmail} readOnly />
          <span className="icon-inside">
            <i className="bi bi-lock"></i>
          </span>
        </div>
        <label htmlFor="fNameInput">Fornavn</label>
        <input type="text" id="fNameInput" />
        <label htmlFor="lNameInput">Etternavn</label>
        <input type="text" id="lNameInput" />
        <label htmlFor="dobInput">Fødselsdato</label>
        <input className="date" type="date" id="dobInput" />
      </div>
      <div className="allergyContainer">
        <label className="allergies" htmlFor="allergyInput">
          Allergier
        </label>
        <div className="allergyBtnDiv">
          <input
            type="text"
            id="allergyInput"
            onChange={handleChange}
            value={newAllergy}
          />
          <button className="addBtn" onClick={handleClick}>
            Legg til
          </button>
        </div>
        <label className="yourAllergies">Dine allergier</label>
        <div className="allergyOutput">
          <ul>
            {allergies.map((item, i) => (
              <li className="allergy" key={i}>
                {item}
                <i className="trash bi bi-trash3 "></i>
              </li>
            ))}
          </ul>
        </div>
        <div className="disclaimerDiv">
          <input className="checkBox" type="checkbox" />
          <label className="disclaimer">
            Ved å registrere brukerprofil, samtykker du til at denne
            informasjonen kan deles med arrangør og eventuelle medarrangører.
          </label>
        </div>
        <div className="btnDiv">
          <button className="canclBtn">Avbryt</button>
          <button className="createBtn">Opprett</button>
        </div>
      </div>
    </div>
  );
};
