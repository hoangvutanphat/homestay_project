import React from "react";
import { Card, CardContent } from "@/components/ui/card";
import { Shield, Clock, Heart, Star, Award, Headphones } from "lucide-react";

const features = [
  {
    icon: <Shield className="w-8 h-8" />,
    title: "Bảo mật tuyệt đối",
    description:
      "Thông tin cá nhân và thanh toán được bảo vệ bằng công nghệ mã hóa tiên tiến",
    color: "from-blue-500 to-blue-600",
    bgColor: "from-blue-50 to-blue-100",
  },
  {
    icon: <Clock className="w-8 h-8" />,
    title: "Đặt phòng nhanh chóng",
    description:
      "Chỉ cần 3 bước đơn giản để hoàn tất việc đặt phòng trong vài phút",
    color: "from-indigo-500 to-indigo-600",
    bgColor: "from-indigo-50 to-indigo-100",
  },
  {
    icon: <Heart className="w-8 h-8" />,
    title: "Dịch vụ tận tâm",
    description: "Đội ngũ chăm sóc khách hàng nhiệt tình, sẵn sàng hỗ trợ 24/7",
    color: "from-purple-500 to-purple-600",
    bgColor: "from-purple-50 to-purple-100",
  },
  {
    icon: <Star className="w-8 h-8" />,
    title: "Chất lượng đảm bảo",
    description:
      "Tất cả chỗ nghỉ đều được kiểm duyệt kỹ lưỡng về chất lượng và tiện nghi",
    color: "from-violet-500 to-violet-600",
    bgColor: "from-violet-50 to-violet-100",
  },
  {
    icon: <Award className="w-8 h-8" />,
    title: "Giá tốt nhất",
    description: "Cam kết mang đến mức giá cạnh tranh nhất trên thị trường",
    color: "from-cyan-500 to-cyan-600",
    bgColor: "from-cyan-50 to-cyan-100",
  },
  {
    icon: <Headphones className="w-8 h-8" />,
    title: "Hỗ trợ đa kênh",
    description:
      "Liên hệ qua điện thoại, email, chat trực tuyến hoặc ứng dụng di động",
    color: "from-sky-500 to-sky-600",
    bgColor: "from-sky-50 to-sky-100",
  },
];

const WhyChooseUs = () => {
  return (
    <section className="container py-20 px-20 relative overflow-hidden">
      <div className="text-center mb-16 relative z-10">
        <div className="inline-flex items-center px-4 py-2 bg-gradient-to-r from-blue-100 to-indigo-100 rounded-full mb-4">
          <Award className="w-5 h-5 text-blue-600 mr-2" />
          <span className="text-blue-600 font-semibold">Ưu điểm vượt trội</span>
        </div>
        <h2 className="text-4xl md:text-5xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent mb-4 leading-tight pb-2">
          Tại sao chọn BlissStay?
        </h2>
        <p className="text-xl text-gray-600 max-w-3xl mx-auto leading-relaxed">
          Chúng tôi cam kết mang đến trải nghiệm đặt phòng hoàn hảo với những
          giá trị cốt lõi
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 relative z-10">
        {features.map((feature, index) => (
          <Card
            key={index}
            className="group border-0 shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-2 overflow-hidden relative"
          >
            <div
              className={`absolute inset-0 bg-gradient-to-br ${feature.bgColor} opacity-50 group-hover:opacity-70 transition-opacity duration-300`}
            ></div>
            <CardContent className="p-8 h-full text-center relative z-10">
              <div
                className={`inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-gradient-to-r ${feature.color} text-white mb-6 group-hover:scale-110 transition-transform duration-300 shadow-lg`}
              >
                {feature.icon}
              </div>
              <h3 className="text-xl font-bold mb-4 text-gray-800">
                {feature.title}
              </h3>
              <p className="text-gray-600 leading-relaxed">
                {feature.description}
              </p>
            </CardContent>
          </Card>
        ))}
      </div>
      <div className="mt-20 bg-gradient-to-r from-blue-50 via-indigo-50 to-purple-50 rounded-3xl p-8 md:p-12 shadow-2xl border border-blue-100 relative z-10">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-8 text-center">
          <div className="group">
            <div className="text-3xl md:text-4xl font-bold bg-gradient-to-r from-blue-600 to-blue-700 bg-clip-text text-transparent mb-2 group-hover:scale-110 transition-transform duration-300">
              5K+
            </div>
            <p className="text-gray-600 font-medium">Khách hàng hài lòng</p>
          </div>
          <div className="group">
            <div className="text-3xl md:text-4xl font-bold bg-gradient-to-r from-indigo-600 to-indigo-700 bg-clip-text text-transparent mb-2 group-hover:scale-110 transition-transform duration-300">
              1000+
            </div>
            <p className="text-gray-600 font-medium">Chỗ nghỉ chất lượng</p>
          </div>
          <div className="group">
            <div className="text-3xl md:text-4xl font-bold bg-gradient-to-r from-purple-600 to-purple-700 bg-clip-text text-transparent mb-2 group-hover:scale-110 transition-transform duration-300">
              4.8★
            </div>
            <p className="text-gray-600 font-medium">Đánh giá trung bình</p>
          </div>
          <div className="group">
            <div className="text-3xl md:text-4xl font-bold bg-gradient-to-r from-violet-600 to-violet-700 bg-clip-text text-transparent mb-2 group-hover:scale-110 transition-transform duration-300">
              24/7
            </div>
            <p className="text-gray-600 font-medium">Hỗ trợ khách hàng</p>
          </div>
        </div>
      </div>
    </section>
  );
};

export default WhyChooseUs;
