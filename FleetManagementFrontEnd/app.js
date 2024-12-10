let map
let markers = []
let vehicleDetailsModal = document.getElementById('vehicle-details')
let vehicleInfo = document.getElementById('vehicle-info')
let closeModalButton = document.getElementById('close-btn')

// Initialize Google Map
function initMap() {
	map = new google.maps.Map(document.getElementById('map-container'), {
		zoom: 5,
		center: { lat: -25.7479, lng: 28.2293 }, // Default location (Pretoria)
	})

	// Fetch and display vehicle locations
	fetchVehicleLocations()
}

// Fetch vehicle locations from the backend API
async function fetchVehicleLocations() {
	try {
		const response = await fetch()
		//'https://localhost:44361/api/vehicles/locations'
		const data = await response.json()

		// Display vehicle locations on the map
		displayVehicleMarkers(data)
	} catch (error) {
		console.error('Error fetching vehicle locations:', error)
	}
}

// Display markers on the map for each vehicle
function displayVehicleMarkers(locations) {
	locations.forEach((location) => {
		const position = { lat: location.latitude, lng: location.longitude }

		// Create marker
		const marker = new google.maps.Marker({
			position: position,
			map: map,
			title: `Vehicle ID: ${location.vehicleRegistrationNumber}`,
			vehicleData: location, // Store the vehicle data with the marker
		})

		// Add marker to array
		markers.push(marker)

		// Add click event to show vehicle details
		marker.addListener('click', () => {
			showVehicleDetails(marker.vehicleData)
		})
	})
}

// Show vehicle details in the modal
function showVehicleDetails(vehicle) {
	vehicleInfo.innerHTML = `
    <strong>Vehicle ID:</strong> ${vehicle.vehicleID}<br>
    <strong>Registration Number:</strong> ${
			vehicle.vehicleRegistrationNumber
		}<br>
    <strong>Latitude:</strong> ${vehicle.latitude}<br>
    <strong>Longitude:</strong> ${vehicle.longitude}<br>
    <strong>Timestamp:</strong> ${new Date(vehicle.timestamp).toLocaleString()}
  `

	// Show the modal
	vehicleDetailsModal.style.display = 'block'
}

// Close the modal
closeModalButton.addEventListener('click', () => {
	vehicleDetailsModal.style.display = 'none'
})

// Close the modal if clicked outside
window.addEventListener('click', (event) => {
	if (event.target === vehicleDetailsModal) {
		vehicleDetailsModal.style.display = 'none'
	}
})

// Call initMap() when Google Maps API is ready
window.initMap = initMap
