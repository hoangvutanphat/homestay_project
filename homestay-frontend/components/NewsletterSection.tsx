import React from "react";
import { Card, CardContent } from "@/components/ui/card";
import { Mail, Gift, Sparkles, TrendingUp, Users, Award } from "lucide-react";

const benefits = [
  {
    icon: <Gift className="w-8 h-8" />,
    title: "Ưu đãi độc quyền",
    description:
      "Nhận mã giảm giá lên đến 50% và các chương trình khuyến mãi đặc biệt chỉ dành cho thành viên",
    color: "from-blue-500 to-blue-600",
    bgColor: "from-blue-50 to-blue-100",
  },
  {
    icon: <Sparkles className="w-8 h-8" />,
    title: "Tích điểm thưởng",
    description:
      "Mỗi lần đặt phòng đều được tích điểm và đổi thành phiếu quà tặng hấp dẫn",
    color: "from-purple-500 to-purple-600",
    bgColor: "from-purple-50 to-purple-100",
  },
  {
    icon: <TrendingUp className="w-8 h-8" />,
    title: "Ưu tiên nâng hạng",
    description:
      "Được ưu tiên nâng hạng phòng miễn phí khi có sẵn tại homestay",
    color: "from-indigo-500 to-indigo-600",
    bgColor: "from-indigo-50 to-indigo-100",
  },
  {
    icon: <Users className="w-8 h-8" />,
    title: "Cộng đồng du lịch",
    description:
      "Tham gia cộng đồng yêu thích du lịch, chia sẻ trải nghiệm và nhận tư vấn",
    color: "from-cyan-500 to-cyan-600",
    bgColor: "from-cyan-50 to-cyan-100",
  },
  {
    icon: <Mail className="w-8 h-8" />,
    title: "Tin tức du lịch",
    description:
      "Cập nhật xu hướng du lịch, điểm đến hot và mẹo tiết kiệm chi phí",
    color: "from-violet-500 to-violet-600",
    bgColor: "from-violet-50 to-violet-100",
  },
  {
    icon: <Award className="w-8 h-8" />,
    title: "Dịch vụ VIP",
    description:
      "Hỗ trợ ưu tiên 24/7, tư vấn chuyên nghiệp và chăm sóc khách hàng đặc biệt",
    color: "from-sky-500 to-sky-600",
    bgColor: "from-sky-50 to-sky-100",
  },
];

const NewsletterSection = () => {
  return (
    <section className="container py-20 px-6 md:px-20 relative overflow-hidden">
      <div className="text-center mb-16 relative z-10">
        <div className="inline-flex items-center px-4 py-2 bg-gradient-to-r from-blue-100 to-purple-100 rounded-full mb-4">
          <Sparkles className="w-5 h-5 text-blue-600 mr-2" />
          <span className="text-blue-600 font-semibold">
            Đặc quyền thành viên
          </span>
        </div>
        <h2 className="text-4xl md:text-5xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent leading-tight pb-2 mb-4">
          Trở thành thành viên BlissStay
        </h2>
        <p className="text-xl text-gray-600 max-w-3xl mx-auto leading-relaxed">
          Tận hưởng những đặc quyền và ưu đãi độc quyền dành riêng cho cộng đồng
          BlissStay
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 relative z-10">
        {benefits.map((benefit, index) => (
          <Card
            key={index}
            className="group border-0 shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-2 overflow-hidden h-full"
          >
            <CardContent className="p-8 text-center relative h-full">
              <div
                className={`absolute inset-0 bg-gradient-to-br ${benefit.bgColor} opacity-50 group-hover:opacity-70 transition-opacity duration-300`}
              ></div>
              <div className="relative z-10 flex flex-col h-full">
                <div
                  className={`inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-gradient-to-r ${benefit.color} text-white mb-6 group-hover:scale-110 transition-transform duration-300 shadow-lg mx-auto`}
                >
                  {benefit.icon}
                </div>
                <h3 className="text-xl font-bold mb-4 text-gray-800">
                  {benefit.title}
                </h3>
                <p className="text-gray-600 leading-relaxed">
                  {benefit.description}
                </p>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    </section>
  );
};

export default NewsletterSection;
