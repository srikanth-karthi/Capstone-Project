import { showToast } from "../../Package/Package.js";
import { fetchData } from "../../Package/api.js";
import { toggleDisplay } from "../../Package/Domtools.js";

var inst = mobiscroll.eventcalendar('#demo-desktop-month-view', {
  theme: 'ios',
  themeVariant: 'light',
  clickToCreate: false,
  dragToCreate: false,
  dragToMove: false,
  dragToResize: false,
  eventDelete: false,
  view: {
      calendar: { labels: true },
  },
  onEventClick: function (args) {
      mobiscroll.toast({
          message: args.event.title,
      });
  },
});

const data=[
  {
      "scheduledEventId": 5,
      "eventCategory": "birthday party",
      "venueType": "OwnVenue",
      "locationDetails": "ergerg",
      "foodPreference": "Veg",
      "cateringInstructions": "erger",
      "specialInstructions": "ergerg",
      "eventStartDate": "2024-07-28T10:15:00",
      "eventEndDate": "2024-07-31T10:15:00",
      "requestDate": "2024-07-26T15:45:29.2725906",
      "isCompleted": false,
      "orderId": 3,
      "isReviewed": false
  },
  {
    "scheduledEventId": 5,
    "eventCategory": "birthday party",
    "venueType": "OwnVenue",
    "locationDetails": "ergerg",
    "foodPreference": "Veg",
    "cateringInstructions": "erger",
    "specialInstructions": "ergerg",
    "eventStartDate": "2024-07-28T10:15:00",
    "eventEndDate": "2024-07-31T10:15:00",
    "requestDate": "2024-07-26T15:45:29.2725906",
    "isCompleted": false,
    "orderId": 3,
    "isReviewed": false
},
  {
      "scheduledEventId": 1,
      "eventCategory": "birthday party",
      "venueType": "OwnVenue",
      "locationDetails": "My location details",
      "foodPreference": "Both",
      "cateringInstructions": "both veg and non veg",
      "specialInstructions": "No special instructions",
      "eventStartDate": "2024-08-02T06:00:00.396",
      "eventEndDate": "2024-08-08T08:00:23.396",
      "requestDate": "2024-07-24T17:45:56.9487542",
      "isCompleted": false,
      "orderId": 1,
      "isReviewed": false
  },
  {
      "scheduledEventId": 3,
      "eventCategory": "birthday party",
      "venueType": "OwnVenue",
      "locationDetails": "My location details",
      "foodPreference": "Both",
      "cateringInstructions": "both veg and non veg",
      "specialInstructions": "No special instructions",
      "eventStartDate": "2024-08-02T06:00:00.396",
      "eventEndDate": "2024-08-05T08:00:23.396",
      "requestDate": "2024-07-24T21:09:13.8324296",
      "isCompleted": false,
      "orderId": 2,
      "isReviewed": false
  }
]
      const events = data.map(event => ({
          start: new Date(event.eventStartDate).toISOString(),
          end: new Date(event.eventEndDate).toISOString(),
          title: event.eventCategory,
          color: '#ff6d42'
      }));
      inst.setEvents(events);
