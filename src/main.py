import requests
import psycopg2

# Domain API setup
API_KEY = "key_ec86162104b1287b23f680f4eb8124fe"  # Replace with your actual Domain API key
BASE_URL = "https://api.domain.com.au/v1/propertyenrichment"  # Corrected endpoint

print("Script is running")

def fetch_data():
    """Fetch data from Domain API."""
    # Define query parameters
    params = {
        "propertyId": "CU-5170-IQ",  # Example parameter
    }
    
    # Define headers
    headers = {
        "X-Api-Key":API_KEY
    }
    
    print(f"Using API Key: {API_KEY}")
    print(f"Headers: {headers}")
    
    # Make the API call
    print("Before API call")
    response = requests.get(BASE_URL, headers=headers, params=params)
    print("After API call")
    
    if response.status_code == 200:
        return response.json()  # Return the API response as JSON
    else:
        print(f"Error: {response.status_code} - {response.text}")
        return None


def save_to_postgres(data):
    """Save fetched data into a PostgreSQL database."""
    if not data:
        print("No data to save.")
        return

    # Ensure data is a list
    if isinstance(data, dict):
        data = [data]

    try:
        # Connect to PostgreSQL
        conn = psycopg2.connect(
            dbname="PropertyAPI",  # Replace with your database name
            user="postgres",        # Replace with your username
            password="Scotl@nd07",    # Replace with your password
            host="localhost",
            port="5433"
        )
        cursor = conn.cursor()
        
        # Parse and insert data
        for item in data:
            propertyID = item.get("propertyID")
            address_components = item.get("addressComponents", {})
            flatNumber = address_components.get("flatNumber", "Unknown")
            streetNumber = address_components.get("streetNumber", "Unknown")
            streetName = address_components.get("streetName", "Unknown")
            streetType = address_components.get("streetType", "Unknown")
            streetTypeLong = address_components.get("streetTypeLong", "Unknown")
            suburb = address_components.get("suburb", "Unknown")
            state = address_components.get("state", "Unknown")
            postcode = address_components.get("postcode", "Unknown")
            
            propertySummary = item.get("propertySummary", {})
            propertyType = propertySummary.get("propertyType", "Unknown")
            bedrooms = propertySummary.get("bedrooms")
            bathrooms = propertySummary.get("bathrooms")
            carspaces = propertySummary.get("carSpaces")
            landSize = propertySummary.get("landSize")
            buildingSize = propertySummary.get("buildingSize")
            
            activitySummary = item.get("activitySummary", {})
            lastListedDate = activitySummary.get("lastListedDate")
            lastSoldDate = activitySummary.get("lastSoldDate")
            lastRentedDate = activitySummary.get("lastRentedDate")
            lastListedPrice = activitySummary.get("lastListedPrice")
            lastSoldPrice = activitySummary.get("lastSoldPrice")
            lastRentedPrice = activitySummary.get("lastRentedPrice")
            ownerRenter = activitySummary.get("ownerRenter")
            marketStatus = activitySummary.get("marketStatus")
        
            # Insert into database
            cursor.execute("""
            INSERT INTO properties (
                "propertyID", "flatNumber", "streetNumber", "streetName", "streetType", 
                "streetTypeLong", "suburb", "state", "postcode", "propertyType", 
                "bedrooms", "bathrooms", "carSpaces", "landSize", "buildingSize",
                "lastListedDate", "lastSoldDate", "lastRentedDate",
                "lastListedPrice", "lastSoldPrice", "lastRentedPrice",
                "ownerRenter", "marketStatus"
            ) VALUES (
                %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, 
                %s, %s, %s, %s, %s, %s, %s, %s, %s, %s,
                %s, %s, %s
            )
        """, (
            propertyID, flatNumber, streetNumber, streetName, streetType,
            streetTypeLong, suburb, state, postcode, propertyType,
            bedrooms, bathrooms, carspaces, landSize, buildingSize,
            lastListedDate, lastSoldDate, lastRentedDate,
            lastListedPrice, lastSoldPrice, lastRentedPrice,
            ownerRenter, marketStatus
        ))
            # Commit after each insert
            conn.commit()
            print("Data saved successfully.")
    except Exception as e:
        print(f"Error saving data to PostgreSQL: {e}")
    finally:
        cursor.close()
        conn.close()


if __name__ == "__main__":
    # Fetch data from Domain API
    data = fetch_data()
    if data:
        # Save data to PostgreSQL
        save_to_postgres(data)
