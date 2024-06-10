import "./temp.css";
import "./eventPage.css";
import { Routes, Route } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { MainNav } from "./components/shared/MainNav";
import { HomePage } from "./pages/HomePage";
import { ProfilePage } from "./pages/ProfilePage";
import { TicketsPage } from "./pages/TicketsPage";
import { EventsPage } from "./pages/EventsPage";
import { CreateEventPage } from "./pages/CreateEventPage";
import { OrgProfilePage } from "./pages/OrgProfilePage";
import { EventDetails } from "./pages/EventDetails";
import { LoginPage } from "./pages/LoginPage";
import { useEffect, useState } from "react";
import { RegisterUserProfilePage } from "./pages/RegisterUserProfilePage";

function App() {
  const navigate = useNavigate();
  const [navBar, setNavBar] = useState("dummy");
  const fetchUser = async () => {
    const loggedIn = localStorage.getItem("loggedIn");
    if (loggedIn === "true") {
      if (localStorage.getItem("admin") === "true") {
        setNavBar("organizer");
      } else if (localStorage.getItem("organizer") === "true") {
        setNavBar("organizer");
      } else {
        setNavBar("user");
      }
    } else {
      navigate("login");
    }
  };
  useEffect(() => {
    const handleStorageChange = () => {
      fetchUser();
    };

    window.addEventListener("storage", handleStorageChange);
    fetchUser();
    return () => {
      window.removeEventListener("storage", handleStorageChange);
    };
  }, []);
  return (
    <>
      <MainNav version={navBar} />
      <main>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="profile" element={<ProfilePage />} />
          <Route path="tickets" element={<TicketsPage />} />
          <Route path="events" element={<EventsPage />} />
          <Route path="createEvent" element={<CreateEventPage />} />
          <Route path="events/details/:id" element={<EventDetails />} />
          <Route path="orgProfile" element={<OrgProfilePage />} />
          <Route path="login" element={<LoginPage />} />
          <Route path="register" element={<RegisterUserProfilePage />} />
        </Routes>
      </main>
    </>
  );
}

export default App;
