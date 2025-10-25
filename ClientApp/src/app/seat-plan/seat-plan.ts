import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router'; // Import Router
import { FormsModule, NgForm } from '@angular/forms';
import { BookingService, SeatPlanDto, SeatDto, BookSeatInputDto } from '../services/booking';

@Component({
  selector: 'app-seat-plan',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './seat-plan.html',
  styleUrls: ['./seat-plan.css']
})
export class SeatPlan implements OnInit {

  // Data
  seatPlan: SeatPlanDto | null = null;
  isLoading = true;
  errorMessage: string | null = null;

  // Booking Form
  selectedSeat: SeatDto | null = null;
  passengerName = '';
  passengerMobile = '';
  boardingPoint = 'Kallyanpur'; // Default boarding point
  droppingPoint = 'Rajshahi Counter'; // Default dropping point

  // Booking State
  isBooking = false;
  bookingResult: { success: boolean, message: string } | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router, // Inject Router for navigation
    private bookingService: BookingService
  ) { }

  ngOnInit(): void {
    // 1. Get scheduleId from the URL
    const scheduleId = this.route.snapshot.paramMap.get('scheduleId');

    if (scheduleId) {
      // 2. Fetch the seat plan
      this.bookingService.getSeatPlan(scheduleId).subscribe({
        next: (data) => {
          this.seatPlan = data;
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Could not load seat plan. Please try again.';
          this.isLoading = false;
        }
      });
    } else {
      // Handle case where no ID is provided
      this.errorMessage = 'No schedule ID provided.';
      this.isLoading = false;
    }
  }

  // 3. Handle clicking on a seat
  selectSeat(seat: SeatDto): void {
    if (seat.status === 'Available') {
      this.selectedSeat = seat;
      this.bookingResult = null; // Clear old booking results
    }
  }

  // 4. Handle the "Confirm Booking" submission
  onBookingSubmit(form: NgForm): void {
    if (form.invalid || !this.selectedSeat || !this.seatPlan) {
      return;
    }

    this.isBooking = true;
    this.bookingResult = null;

    const input: BookSeatInputDto = {
      busScheduleId: this.seatPlan.busScheduleId,
      seatId: this.selectedSeat.seatId,
      passengerName: this.passengerName,
      passengerMobile: this.passengerMobile,
      boardingPoint: this.boardingPoint,
      droppingPoint: this.droppingPoint
    };

    // 5. Call the booking service
    this.bookingService.bookSeat(input).subscribe({
      next: (result) => {
        this.isBooking = false;
        if (result.success) {
          this.bookingResult = { success: true, message: `Success! Seat ${result.seatNumber} is booked. Ticket ID: ${result.ticketId}` };
          // Update the UI
          if (this.selectedSeat) {
            this.selectedSeat.status = 'Booked';
            this.selectedSeat = null; // Clear selection
          }
        } else {
          this.bookingResult = { success: false, message: result.message };
        }
      },
      error: (err) => {
        this.isBooking = false;
        this.bookingResult = { success: false, message: err.error.message || 'An error occurred during booking.' };
      }
    });
  }

  // Helper to go back to search
  goBack(): void {
    this.router.navigate(['/']); // Navigate to home
  }
}