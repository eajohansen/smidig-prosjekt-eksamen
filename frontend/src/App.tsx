import "./temp.css";
import axios from "axios";
import LoginPopup from "./components/LoginPopup";

function App() {
  async function testClick() {
    const response = await axios.post("http://localhost:5500/test");
    console.log(response);
  }

  return (
    <>
      <LoginPopup />
      {/* <button onClick={() => testClick()}> Test </button> */}
    </>
  );
}

export default App;
