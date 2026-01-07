"use client";

import { useState, useEffect } from "react";
import { Card, CardContent } from "@/components/ui/card";
import { Star } from "lucide-react";

const testimonials = [
  {
    id: 1,
    name: "Nguyễn Văn Anh",
    role: "Doanh nhân",
    content:
      "Tôi đã tìm thấy homestay hoàn hảo cho chuyến công tác nhờ BlissStay. Giao diện dễ sử dụng và thông tin chi tiết giúp tôi đưa ra quyết định nhanh chóng.",
    rating: 5,
    image:
      "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1170&q=80",
  },
  {
    id: 2,
    name: "Trần Thị Mai",
    role: "Du khách",
    content:
      "Ứng dụng tuyệt vời! Tôi đã book được một biệt thự view biển tuyệt đẹp ở Đà Nẵng với giá rẻ hơn các nền tảng khác. Chắc chắn sẽ sử dụng BlissStay cho các chuyến đi tiếp theo.",
    rating: 5,
    image:
      "https://images.unsplash.com/photo-1494790108377-be9c29b29330?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80",
  },
  {
    id: 3,
    name: "Lê Minh Đức",
    role: "Nhiếp ảnh gia",
    content:
      "Hình ảnh thực tế đúng với những gì được hiển thị trên website. Dịch vụ khách hàng rất tốt, họ đã giúp tôi thay đổi đặt phòng vào phút chót.",
    rating: 4,
    image:
      "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=387&q=80",
  },
  {
    id: 4,
    name: "Phạm Hoàng Yến",
    role: "Giáo viên",
    content:
      "Lần đầu tiên tôi sử dụng BlissStay và thật sự ấn tượng. Thanh toán an toàn và nhận được xác nhận ngay lập tức. Chỗ ở tuyệt vời và đúng như mô tả.",
    rating: 5,
    image:
      "https://images.unsplash.com/photo-1580489944761-15a19d654956?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=461&q=80",
  },
];

const Testimonials = () => {
  const [activeIndex, setActiveIndex] = useState(0);
  const itemsShown = 2;
  const totalItems = testimonials.length;

  useEffect(() => {
    const timer = setTimeout(() => {
      setActiveIndex((current) => (current + 1) % (totalItems - 1));
    }, 5000);

    return () => clearTimeout(timer);
  }, [activeIndex, totalItems]);

  return (
    <section className="container py-20 px-6 md:px-20 relative overflow-hidden">
      <div className="text-center mb-16 relative z-10">
        <div className="inline-flex items-center px-4 py-2 bg-gradient-to-r from-blue-100 to-purple-100 rounded-full mb-4">
          <Star className="w-5 h-5 text-blue-600 mr-2" />
          <span className="text-blue-600 font-semibold">
            Đánh giá của khách hàng
          </span>
        </div>
        <h2 className="text-4xl md:text-5xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent leading-tight pb-2 mb-4">
          Khách hàng nói gì về chúng tôi
        </h2>
        <p className="text-xl text-gray-600 max-w-3xl mx-auto leading-relaxed">
          Trải nghiệm thực tế từ những khách hàng đã sử dụng dịch vụ của
          BlissStay
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-8 relative z-10">
        {testimonials
          .slice(activeIndex, activeIndex + itemsShown)
          .map((testimonial) => (
            <Card
              key={testimonial.id}
              className="group overflow-hidden h-[240px] border-0 shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-2"
            >
              <CardContent className="p-6 h-full flex flex-col relative">
                <div className="absolute inset-0 bg-gradient-to-br from-blue-50 to-purple-50 opacity-50 group-hover:opacity-70 transition-opacity duration-300"></div>
                <div className="relative z-10 flex flex-col h-full">
                  <div className="flex items-center gap-4 mb-4">
                    <img
                      src={testimonial.image}
                      alt={testimonial.name}
                      className="w-16 h-16 rounded-full object-cover ring-4 ring-white shadow-lg"
                    />
                    <div>
                      <h4 className="font-bold text-gray-800">
                        {testimonial.name}
                      </h4>
                      <p className="text-sm text-gray-600">
                        {testimonial.role}
                      </p>
                      <div className="flex items-center mt-1">
                        {Array.from({ length: 5 }).map((_, i) => (
                          <Star
                            key={i}
                            className={`h-4 w-4 ${
                              i < testimonial.rating
                                ? "fill-yellow-400 text-yellow-400"
                                : "text-gray-300"
                            }`}
                          />
                        ))}
                      </div>
                    </div>
                  </div>
                  <p className="text-gray-600 italic line-clamp-4 leading-relaxed">
                    "{testimonial.content}"
                  </p>
                </div>
              </CardContent>
            </Card>
          ))}
      </div>

      <div className="flex justify-center mt-12">
        {Array.from({ length: totalItems - 1 }).map((_, index) => (
          <button
            key={index}
            onClick={() => setActiveIndex(index)}
            className={`w-3 h-3 rounded-full mx-1.5 transition-all duration-300 ${
              index === activeIndex
                ? "bg-gradient-to-r from-blue-500 to-purple-500 scale-125"
                : "bg-gray-300 hover:bg-gray-400"
            }`}
            aria-label={`Go to testimonial set ${index + 1}`}
          />
        ))}
      </div>
    </section>
  );
};

export default Testimonials;
