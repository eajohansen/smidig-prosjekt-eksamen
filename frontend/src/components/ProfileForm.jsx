import { useState, useEffect, SyntheticEvent } from "react";
import { sendUser } from "../services/tempService";
export const ProfileForm = ({ userEmail }) => {
  const [allergies, setAllergies] = useState([]);
  const [newAllergy, setNewAllergy] = useState("");

  const [user, setUser] = useState({
    firstName: "",
    lastName: "",
    email: userEmail,
  });

  useEffect(() => {
    setNewAllergy(newAllergy);
  }, [newAllergy]);

  useEffect(() => {
    setUser(user);
  }, [user]);

  const handleChange = (e) => {
    const value = e.currentTarget.value;
    switch (e.currentTarget.name) {
      case "fName":
        setUser({ ...user, firstName: value });
        console.log(value);
        break;
      case "lName":
        setUser({ ...user, lastName: value });
        console.log(value);
        break;
      case "dob":
        break;
      case "allergies":
        setNewAllergy(value);
        break;
    }
  };

  const handleAllergy = () => {
    setAllergies([...allergies, newAllergy]);
    setNewAllergy("");
  };

  const handleSubmit = async () => {
    const result = await sendUser(user);
    console.log(result);
  };

  return (
    <div className="profileInfoContainer formContainer">
      <div className="userInfoContainer">
        <h2>Opprett Bruker</h2>
        <label htmlFor="emailRegister">Epost</label>
        <input type="email" id="emailRegister" value={userEmail} readOnly />
        <label htmlFor="fNameInput">Fornavn</label>
        <input
          type="text"
          id="fNameInput"
          name="fName"
          onChange={handleChange}
        />
        <label htmlFor="lNameInput">Etternavn</label>
        <input
          type="text"
          id="lNameInput"
          name="lName"
          onChange={handleChange}
        />
        <label htmlFor="dobInput">Fødselsdato</label>
        <input type="date" id="dobInput" name="dob" />
      </div>
      <div className="allergyContainer">
        <h2>Opprett Bruker</h2>

        <label htmlFor="allergyInput">Allergier</label>
        <div className="allergyBtnDiv">
          <input
            type="text"
            id="allergyInput"
            name="allergies"
            onChange={handleChange}
            value={newAllergy}
          />
          <button onClick={handleAllergy}>Legg til</button>
        </div>
        <label>Dine allergier</label>
        <label>Disclaimer...</label>
        <div className="allergyOutput">
          <ul>
            {allergies.map((item, i) => (
              <li key={i}>{item}</li>
            ))}
          </ul>
        </div>
        <div className="btnDiv">
          <button>Avbryt</button>
          <button
            onClick={() => {
              handleSubmit();
            }}
          >
            Opprett
          </button>
        </div>
      </div>
    </div>
  );
};
