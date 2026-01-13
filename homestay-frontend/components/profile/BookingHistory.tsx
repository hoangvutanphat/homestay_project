// import React from "react";
// import { ExternalLink, Calendar } from "lucide-react";
// import {
//   Card,
//   CardContent,
//   CardDescription,
//   CardHeader,
//   CardTitle,
// } from "@/components/ui/card";
// import {
//   Table,
//   TableBody,
//   TableCell,
//   TableHead,
//   TableHeader,
//   TableRow,
// } from "@/components/ui/table";
// import { Badge } from "@/components/ui/badge";
// import { ScrollArea } from "@/components/ui/scroll-area";
// import { Button } from "@/components/ui/button";
// import { useRouter } from "next/navigation";
// import { useBookings } from "@/hooks/useBookings";
// import { useEffect } from "react";

// const getStatusColor = (status: string) => {
//   switch (status) {
//     case "completed":
//       return "bg-green-500 hover:bg-green-600";
//     case "confirmed":
//       return "bg-blue-500 hover:bg-blue-600";
//     case "pending":
//       return "bg-yellow-500 hover:bg-yellow-600";
//     case "cancelled":
//       return "bg-red-500 hover:bg-red-600";
//     default:
//       return "bg-gray-500 hover:bg-gray-600";
//   }
// };

// const statusMapping: Record<string, string> = {
//   pending: "Đang chờ",
//   confirmed: "Đã xác nhận",
//   cancelled: "Đã hủy",
//   completed: "Hoàn thành",
// };

// const BookingHistory: React.FC = () => {
//   const router = useRouter();
// //   const { bookings, loading, error, fetchBookings } = useBookings();

//   useEffect(() => {
//     fetchBookings();
//   }, []);

//   const handleViewInvoice = (booking: any) => {
//     // TODO: Implement invoice detail modal
//     console.log("View invoice:", booking);
//   };

//   const needsScrolling = bookings.length > 7;

//   if (loading) {
//     return (
//       <Card className="mb-6">
//         <CardHeader>
//           <CardTitle>Lịch sử đặt phòng</CardTitle>
//         </CardHeader>
//         <CardContent>
//           <div className="flex items-center justify-center py-8">
//             <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-brand-blue"></div>
//           </div>
//         </CardContent>
//       </Card>
//     );
//   }

//   if (error) {
//     return (
//       <Card className="mb-6">
//         <CardHeader>
//           <CardTitle>Lịch sử đặt phòng</CardTitle>
//         </CardHeader>
//         <CardContent>
//           <p className="text-center text-red-500 py-8">{error}</p>
//         </CardContent>
//       </Card>
//     );
//   }

//   return (
//     <Card className="mb-6">
//       <CardHeader>
//         <CardTitle>Lịch sử đặt phòng</CardTitle>
//         <CardDescription>
//           Xem thông tin các đơn đặt phòng của bạn
//         </CardDescription>
//       </CardHeader>
//       <CardContent>
//         {bookings.length > 0 ? (
//           <div className="space-y-4">

//             <div className="hidden md:block">
//               <ScrollArea className={needsScrolling ? "h-[400px]" : ""}>
//                 <Table>
//                   <TableHeader>
//                     <TableRow>
//                       <TableHead>Mã đặt phòng</TableHead>
//                       <TableHead>Chỗ ở</TableHead>
//                       <TableHead>Ngày</TableHead>
//                       <TableHead>Giá</TableHead>
//                       <TableHead>Trạng thái</TableHead>
//                       <TableHead>Chi tiết</TableHead>
//                     </TableRow>
//                   </TableHeader>
//                   <TableBody>
//                     {bookings.map((booking) => (
//                       <TableRow key={booking.invoiceCode}>
//                         <TableCell className="font-medium">
//                           {booking.invoiceCode}
//                         </TableCell>
//                         <TableCell>{booking.homestay.name}</TableCell>
//                         <TableCell>
//                           {new Date(booking.checkInDate).toLocaleDateString(
//                             "vi-VN"
//                           )}{" "}
//                           -{" "}
//                           {new Date(booking.checkOutDate).toLocaleDateString(
//                             "vi-VN"
//                           )}
//                         </TableCell>
//                         <TableCell>
//                           {booking.totalPrice.toLocaleString("vi-VN")}đ
//                         </TableCell>
//                         <TableCell>
//                           <Badge
//                             className={getStatusColor(booking.bookingStatus)}
//                           >
//                             {statusMapping[booking.bookingStatus]}
//                           </Badge>
//                         </TableCell>
//                         <TableCell>
//                           <Button
//                             variant="ghost"
//                             size="sm"
//                             className="h-8 w-8 p-0"
//                             onClick={() => handleViewInvoice(booking)}
//                           >
//                             <ExternalLink className="h-4 w-4" />
//                           </Button>
//                         </TableCell>
//                       </TableRow>
//                     ))}
//                   </TableBody>
//                 </Table>
//               </ScrollArea>
//             </div>

//             {/* Mobile view with cards */}
//             <div className="md:hidden">
//               <ScrollArea className={needsScrolling ? "h-[500px]" : ""}>
//                 <div className="grid gap-4">
//                   {bookings.map((booking) => (
//                     <Card key={booking.invoiceCode} className="overflow-hidden">
//                       <div className="flex">
//                         <div className="w-1/3">
//                           <img
//                             src="https://img.freepik.com/free-vector/happy-tourists-choosing-hotel-booking-room-online-flat-illustration_74855-10811.jpg"
//                             alt={booking.homestay.name}
//                             className="h-full w-full object-cover"
//                           />
//                         </div>
//                         <div className="w-2/3 p-4">
//                           <div className="flex justify-between items-start mb-2">
//                             <h3 className="font-semibold text-sm line-clamp-1">
//                               {booking.homestay.name}
//                             </h3>
//                             <Badge
//                               className={getStatusColor(booking.bookingStatus)}
//                             >
//                               {statusMapping[booking.bookingStatus]}
//                             </Badge>
//                           </div>
//                           <p className="text-xs text-gray-500 mb-1">
//                             Mã: {booking.invoiceCode}
//                           </p>
//                           <p className="text-xs text-gray-500 mb-2">
//                             {new Date(booking.checkInDate).toLocaleDateString(
//                               "vi-VN"
//                             )}{" "}
//                             -{" "}
//                             {new Date(booking.checkOutDate).toLocaleDateString(
//                               "vi-VN"
//                             )}
//                           </p>
//                           <div className="flex justify-between items-center">
//                             <span className="font-medium text-sm">
//                               {booking.totalPrice.toLocaleString("vi-VN")}đ
//                             </span>
//                             <Button
//                               variant="outline"
//                               size="sm"
//                               className="h-7 text-xs"
//                               onClick={() => handleViewInvoice(booking)}
//                             >
//                               Chi tiết
//                             </Button>
//                           </div>
//                         </div>
//                       </div>
//                     </Card>
//                   ))}
//                 </div>
//               </ScrollArea>
//             </div>
//           </div>
//         ) : (
//           <div className="text-center py-12 text-gray-500">
//             <Calendar className="mx-auto h-12 w-12 text-gray-300 mb-2" />
//             <p>Bạn chưa có đơn đặt phòng nào</p>
//             <Button
//               variant="outline"
//               className="mt-4"
//               onClick={() => router.push("/search")}
//             >
//               Tìm kiếm chỗ nghỉ
//             </Button>
//           </div>
//         )}
//       </CardContent>
//     </Card>
//   );
// };

// export default BookingHistory;
