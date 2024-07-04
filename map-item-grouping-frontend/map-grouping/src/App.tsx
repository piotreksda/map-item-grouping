import L from 'leaflet';
import H3Map from './H3Map'

function App() {
  return (
    <H3Map center={new L.LatLng(40.741895,-73.989308)} zoom={5}/>
  )
}

export default App
