namespace KirosProject
{
    //represents a solarsystem for a map
    public class SolarSystem
    {
        private string _star; //star's id value
        
        private List<Planet> _planets;
        private List<Moon> _moons;
        private List<Orbital> _orbitals; //asteroids, comets, etc.
        private List<Structure> _structures;
        private List<SapceShip> _ships;
    }
}