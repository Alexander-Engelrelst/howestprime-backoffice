const calendarApp = (() => {
  const MONTH_NAMES = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];
  
  const now = new Date();
  let currentMonth = now.getMonth();
  let currentYear = now.getFullYear();

  const eventsData = {
    2025: {
      4: [
        { day: 1, roomClass: 'event-room1', text: '15:00 Dune (Room I)' },
        { day: 1, roomClass: 'event-room2', text: '19:00 Star Wars (Room II)' },
        { day: 4, roomClass: 'event-room1', text: '15:00 Inception (Room I)' },
      ]
    }
  };

  function initialize() {
    ensureRandomEvents(currentMonth, currentYear);
    renderCalendar(currentMonth, currentYear);
    setupEventListeners();
  }

  function setupEventListeners() {
    const modalForm = document.getElementById('modalForm');
    const addEventModal = document.getElementById('addEventModal');
    
    if (modalForm) {
      modalForm.addEventListener('submit', saveNewEvent);
    }
    
    if (addEventModal) {
      addEventModal.addEventListener('click', function(event) {
        if (event.target === this) {
          hideModal();
        }
      });
    }
    
    const prevButton = document.getElementById('prevMonth');
    const nextButton = document.getElementById('nextMonth');
    
    if (prevButton) prevButton.addEventListener('click', () => changeMonth(-1));
    if (nextButton) nextButton.addEventListener('click', () => changeMonth(1));
  }

  function changeMonth(delta) {
    currentMonth += delta;
    
    if (currentMonth < 0) {
      currentMonth = 11;
      currentYear--;
    } else if (currentMonth > 11) {
      currentMonth = 0;
      currentYear++;
    }
    
    ensureRandomEvents(currentMonth, currentYear);
    renderCalendar(currentMonth, currentYear);
  }

  function ensureRandomEvents(month, year) {
    if (!eventsData[year]) {
      eventsData[year] = {};
    }
    if (!eventsData[year][month]) {
      eventsData[year][month] = [];
    }
    
    const events = eventsData[year][month].filter(event => !event.isRandomEvent);
    
    eventsData[year][month] = events;
    
    if (events.length < 4) {
      const neededEvents = 4 - events.length;
      const randomEvents = generateRandomEvents(month, year, neededEvents);
      eventsData[year][month].push(...randomEvents);
    }
  }
  
  function generateRandomEvents(month, year, count) {
    const movies = ["Avatar", "Inception", "The Matrix", "Star Wars", 
                  "Interstellar", "Jurassic Park", "The Godfather", "Pulp Fiction"];
    const times = ["10:00", "13:00", "16:00", "19:00", "21:30"];
    const rooms = ["event-room1", "event-room2"];
    
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const events = [];
    
    for (let i = 0; i < count; i++) {
      const day = Math.floor(Math.random() * daysInMonth) + 1;
      const movieIndex = Math.floor(Math.random() * movies.length);
      const timeIndex = Math.floor(Math.random() * times.length);
      const roomIndex = Math.floor(Math.random() * rooms.length);
      const roomClass = rooms[roomIndex];
      const roomText = roomClass === 'event-room1' ? 'Room I' : 'Room II';
      
      events.push({
        day,
        roomClass,
        text: `${times[timeIndex]} ${movies[movieIndex]} (${roomText})`,
        isRandomEvent: true
      });
    }
    
    return events;
  }

  function renderCalendar(month, year) {
    updateMonthYearHeader(month, year);
    renderCalendarDays(month, year);
  }

  function updateMonthYearHeader(month, year) {
    const headerElement = document.getElementById("monthYear");
    if (headerElement) {
      headerElement.textContent = `${MONTH_NAMES[month]} ${year}`;
    }
  }

  function renderCalendarDays(month, year) {
    const calendarDays = document.getElementById("calendarDays");
    if (!calendarDays) return;
    
    calendarDays.innerHTML = "";

    const firstDayOfMonth = new Date(year, month, 1).getDay();    
    const daysInMonth = new Date(year, month + 1, 0).getDate();     

    for (let day = 1; day <= daysInMonth; day++) {
      const dayOfWeek = new Date(year, month, day).getDay();
      const offset = firstDayOfMonth + (day - 1);
      const rowNumber = Math.floor(offset / 7); 

      const dayCell = createDayCell(day, dayOfWeek, rowNumber, year, month);
      calendarDays.appendChild(dayCell);
    }
  }

  function createDayCell(day, dayOfWeek, rowNumber, year, month) {
    const dayCell = document.createElement("div");
    dayCell.className = "day-cell";
    dayCell.style.gridColumnStart = (dayOfWeek + 1); 
    dayCell.style.gridRowStart = (rowNumber + 1); 

    const dateSpan = document.createElement("span");
    dateSpan.className = "date-number";
    dateSpan.textContent = day;
    dayCell.appendChild(dateSpan);

    addEventsToDayCell(dayCell, year, month, day);

    dayCell.addEventListener("click", () => {
      openAddEventModal(day, month, year);
    });

    return dayCell;
  }

  function addEventsToDayCell(dayCell, year, month, day) {
    const events = getEventsForDay(year, month, day);
    
    events.forEach(event => {
      const eventDiv = document.createElement("div");
      eventDiv.className = `event ${event.roomClass}`; 
      eventDiv.textContent = event.text;
      dayCell.appendChild(eventDiv);
    });
  }

  function getEventsForDay(year, month, day) {
    if (eventsData[year] && eventsData[year][month]) {
      return eventsData[year][month].filter(e => e.day === day);
    }
    return [];
  }

  function openAddEventModal(day, month, year) {
    const modal = document.getElementById('addEventModal');
    if (!modal) return;

    setFormValue('modalDay', day);
    setFormValue('modalMonth', month);
    setFormValue('modalYear', year);
    setFormValue('modalRoom', '');
    setFormValue('modalTime', '');
    setFormValue('modalMovie', '');

    modal.classList.add('active');
  }

  function setFormValue(id, value) {
    const element = document.getElementById(id);
    if (element) element.value = value;
  }

  function hideModal() {
    const modal = document.getElementById('addEventModal');
    if (modal) modal.classList.remove('active');
  }

  function saveNewEvent(e) {
    e.preventDefault();

    const formData = {
      day: getFormValueAsInt('modalDay'),
      month: getFormValueAsInt('modalMonth'),
      year: getFormValueAsInt('modalYear'),
      roomClass: getFormValue('modalRoom'),
      time: getFormValue('modalTime'),
      movie: getFormValue('modalMovie')
    };

    if (!validateEventForm(formData)) {
      return;
    }

    addEventToData(
      formData.day, 
      formData.month, 
      formData.year, 
      formData.roomClass, 
      formData.time, 
      formData.movie
    );

    hideModal();
    renderCalendar(formData.month, formData.year);
  }

  function validateEventForm(formData) {
    const { roomClass, time, movie } = formData;
    
    if (!roomClass) {
      showFormError('Please select a room');
      return false;
    }
    
    if (!time) {
      showFormError('Please enter a show time');
      return false;
    }
    
    if (!movie) {
      showFormError('Please enter a movie title');
      return false;
    }
    
    return true;
  }

  function getFormValue(id) {
    const element = document.getElementById(id);
    return element ? element.value.trim() : '';
  }

  function getFormValueAsInt(id) {
    return parseInt(getFormValue(id), 10);
  }

  function showFormError(message) {
    const errorElement = document.getElementById('modalError');
    if (errorElement) {
      errorElement.textContent = message;
      errorElement.style.display = 'block';
      setTimeout(() => {
        errorElement.style.display = 'none';
      }, 3000);
    } else {
      console.error(message);
    }
  }

  function addEventToData(day, month, year, roomClass, time, movie) {
    if (!eventsData[year]) {
      eventsData[year] = {};
    }
    if (!eventsData[year][month]) {
      eventsData[year][month] = [];
    }
    
    eventsData[year][month].push({
      day,
      roomClass,
      text: `${time} ${movie} (${roomClass === 'event-room1' ? 'Room I' : 'Room II'})`
    });
  }

  function getAllEvents() {
    return JSON.parse(JSON.stringify(eventsData));
  }

  window.addEventListener("DOMContentLoaded", initialize);

  return {
    changeMonth,
    getEventsForDay,
    getAllEvents,
    renderCalendar
  };
})();
