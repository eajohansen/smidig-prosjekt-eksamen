import "./temp.css";
import axios from "axios";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { MainNav } from "./components/shared/MainNav";
import { HomePage } from "./pages/HomePage";
import { ProfilePage } from "./pages/ProfilePage";
import { TicketsPage } from "./pages/TicketsPage";
import { EventsPage } from "./pages/EventsPage";

function App() {
  async function testClick() {
    const response = await axios.post("http://localhost:5500/test");
    console.log(response);
  }

  return (
    <>
      <BrowserRouter>
        <MainNav version={"user"} />
        <main>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="profile" element={<ProfilePage />} />
            <Route path="tickets" element={<TicketsPage />} />
            <Route path="events" element={<EventsPage />} />
          </Routes>

          {/* <ProfileForm userEmail={"Eksempel@epost.no"} /> */}
          {/* <button onClick={() => testClick()}> Test </button> */}
        </main>
      </BrowserRouter>
    </>
  );
}

export default App;
