import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { BookingService, SeatPlanDto, SeatDto, BookSeatInputDto, StopDto } from '../services/booking';

@Component({
  selector: 'app-seat-plan',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './seat-plan.html',
  styleUrls: ['./seat-plan.css']
})
export class SeatPlan implements OnInit {

  seatPlan: SeatPlanDto | null = null;
  isLoading = true;
  errorMessage: string | null = null;
  seatRows: SeatDto[][] = [];

  selectedSeat: SeatDto | null = null;
  passengerName = '';
  passengerMobile = '';
  boardingPoint: string | null = null;
  droppingPoint: string | null = null;

  isBooking = false;
  bookingResult: { success: boolean, message: string } | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookingService: BookingService
  ) { }

  ngOnInit(): void {
    const scheduleId = this.route.snapshot.paramMap.get('scheduleId');

    if (scheduleId) {
      this.bookingService.getSeatPlan(scheduleId).subscribe({
        next: (data) => {
          this.seatPlan = data;
          this.groupSeatsIntoRows();
          this.isLoading = false;
        },
        error: (err) => {
          console.error("Error loading seat plan:", err);
          this.errorMessage = 'Could not load seat plan. Please try again.';
          this.isLoading = false;
        }
      });
    } else {
      this.errorMessage = 'No schedule ID provided.';
      this.isLoading = false;
    }
  }

  private groupSeatsIntoRows(): void {
    this.seatRows = [];
    if (this.seatPlan?.seats) {
      const sortedSeats = [...this.seatPlan.seats].sort((a, b) => {
        const rowA = a.seatNumber.charAt(0);
        const colA = parseInt(a.seatNumber.substring(1), 10);
        const rowB = b.seatNumber.charAt(0);
        const colB = parseInt(b.seatNumber.substring(1), 10);

        if (rowA < rowB) return -1;
        if (rowA > rowB) return 1;

        return colA - colB;
      });

      for (let i = 0; i < sortedSeats.length; i += 4) {
        this.seatRows.push(sortedSeats.slice(i, i + 4));
      }
    }
  }

  selectSeat(seat: SeatDto): void {
    if (seat.status === 'Available') {
      if (this.selectedSeat?.seatId === seat.seatId) {
        this.selectedSeat = null;
      } else {
        this.selectedSeat = seat;
      }
      this.bookingResult = null;
    }
  }

  onBookingSubmit(form: NgForm): void {
    if (form.invalid || !this.selectedSeat || !this.seatPlan || !this.boardingPoint || !this.droppingPoint) {
      console.error("Form is invalid, seat not selected, or points missing.");
      this.bookingResult = { success: false, message: 'Please select a seat, boarding point, dropping point, and fill in all required fields.' };
      return;
    }

    this.isBooking = true;
    this.bookingResult = null;

    const boardStopName = this.seatPlan.boardingPoints.find(p => p.id === this.boardingPoint)?.name || '';
    const dropStopName = this.seatPlan.droppingPoints.find(p => p.id === this.droppingPoint)?.name || '';

    if (!boardStopName || !dropStopName) {
      console.error("Could not find stop names for selected IDs.");
      this.bookingResult = { success: false, message: 'Invalid boarding or dropping point selected.' };
      this.isBooking = false;
      return;
    }

    const input: BookSeatInputDto = {
      busScheduleId: this.seatPlan.busScheduleId,
      seatId: this.selectedSeat.seatId,
      passengerName: this.passengerName,
      passengerMobile: this.passengerMobile,
      boardingPoint: boardStopName,
      droppingPoint: dropStopName
    };

    this.bookingService.bookSeat(input).subscribe({
      next: (result) => {
        this.isBooking = false;
        if (result.success && this.seatPlan && this.selectedSeat) {
          this.bookingResult = { success: true, message: `Success! Seat ${result.seatNumber} is booked. Ticket ID: ${result.ticketId}` };

          const bookedSeatInPlan = this.seatPlan.seats.find(s => s.seatId === this.selectedSeat?.seatId);

          if (bookedSeatInPlan) {
            bookedSeatInPlan.status = 'Booked';
          }

          this.selectedSeat = null;

          this.groupSeatsIntoRows();

          form.resetForm();

        } else {
          this.bookingResult = { success: false, message: result.message || 'Booking failed. Seat might have been taken.' };
        }
      },
      error: (err) => {
        console.error("Booking API error:", err);
        this.isBooking = false;
        const errorMsg = err?.error?.message || err?.message || 'An unexpected error occurred during booking.';
        this.bookingResult = { success: false, message: errorMsg };
      }
    });
  }

  trackBySeatId(index: number, seat: SeatDto): string {
    return seat.seatId;
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}