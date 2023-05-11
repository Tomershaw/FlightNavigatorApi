import { DateFilterComponent } from "angular2-smart-table/lib/components/filter/filter-types/date-filter.component";

class Flight
{
    constructor(
    public flightnumber = "",
    public leg = 0,
    public isarrival = false,
    public airline = "",
    public createdat ="")
    {

    }
}
export default Flight;